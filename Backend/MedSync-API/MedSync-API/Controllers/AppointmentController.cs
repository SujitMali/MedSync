using MedSync_API.Filter;
using MedSync_ClassLibraries.DAL;
using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace MedSync_API.Controllers
{
    public class AppointmentController : BaseApiController
    {

        [JwtAuthenticate]
        [Authorize(Roles = "Doctor, Admin")]
        [HttpGet]
        public IHttpActionResult GetAllAppointmentStatus()
        {
            try
            {
                AppointmentStatus appointmentStatusDal = new AppointmentStatus();
                var statuses = appointmentStatusDal.GetAppointmentStatusList(CurrentUserId);

                return Ok(new
                {
                    success = true,
                    data = statuses
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "Error fetching appointment statuses: " + ex.Message
                });
            }
        }

        [JwtAuthenticate]
        [Authorize(Roles = "Doctor, Admin")]
        [HttpGet]
        public IHttpActionResult PreviewAppointmentFile(int appointmentId, string fileName)
        {
            try
            {
                if (appointmentId <= 0 || string.IsNullOrEmpty(fileName))
                    return BadRequest("Invalid parameters.");

                string basePath = System.Configuration.ConfigurationManager.AppSettings["AppointmentFilesPath"];
                string folderPath = basePath.Replace("{AppointmentId}", appointmentId.ToString());

                string fullPath = System.IO.Path.Combine(folderPath, fileName);

                if (!System.IO.File.Exists(fullPath))
                    return NotFound();

                string mimeType = MimeMapping.GetMimeMapping(fileName);

                var fileBytes = System.IO.File.ReadAllBytes(fullPath);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(fileBytes)
                };

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
                {
                    FileName = fileName
                };

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [JwtAuthenticate]
        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        public IHttpActionResult GetAppointments([FromBody] AppointmentFilterModel filter)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;

                string role = identity?.FindFirst(ClaimTypes.Role)?.Value;
                string doctorIdClaim = identity?.FindFirst("DoctorID")?.Value;


                if (role.Equals("Doctor", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrEmpty(doctorIdClaim))
                    {
                        return Content(HttpStatusCode.Unauthorized, new
                        {
                            success = false,
                            message = "Doctor ID missing in token."
                        });
                    }

                    filter.DoctorID = int.Parse(doctorIdClaim);
                }

                var appointmentDal = new Appointment();
                DoctorAppointmentsViewModel response = appointmentDal.GetAppointmentsList(filter, CurrentUserId);

                return Ok(new
                {
                    success = true,
                    data = response.Appointments,
                    files = response.AppointmentFiles,
                    totalRecords = response.TotalRecords
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "An error occurred while fetching appointments.",
                    error = ex.Message
                });
            }
        }


        [JwtAuthenticate]
        [Authorize(Roles = "Doctor")]
        [HttpPut]
        public IHttpActionResult UpdateDoctorAppointment([FromBody] AppointmentModel request)
        {
            if (request == null || request.AppointmentID <= 0)
                return BadRequest("Invalid appointment request.");

            try
            {
                var appointmentDal = new Appointment();
                request.ModifiedBy = CurrentUserId;
                var result = appointmentDal.Update(request);

                if (result == null || result.SuccessFlag == 0)
                    return Content(HttpStatusCode.NotFound, "Appointment not found or update failed.");
                if (result.SuccessFlag == -1)
                    return Content(HttpStatusCode.Forbidden, "This appointment can no longer be modified.");


                string subject = $"Appointment {result.AppointmentStatus} - MedSync";
                string patientTemplate = string.Empty;
                string doctorTemplate = string.Empty;

                if (result.AppointmentStatus.Equals("Accepted", StringComparison.OrdinalIgnoreCase))
                {
                    patientTemplate = "AppointmentAccepted_Patient.html";
                    doctorTemplate = "AppointmentAccepted_Doctor.html";
                }
                else if (result.AppointmentStatus.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    patientTemplate = "AppointmentRejected_Patient.html";
                    doctorTemplate = "AppointmentRejected_Doctor.html";
                }

                if (!string.IsNullOrEmpty(patientTemplate) && !string.IsNullOrEmpty(doctorTemplate))
                {
                    string patientBody = emailHelper.LoadEmailTemplate(patientTemplate, result, forDoctor: false);
                    string doctorBody = emailHelper.LoadEmailTemplate(doctorTemplate, result, forDoctor: true);
                    emailHelper.SendEmail(result.PatientEmail, subject, patientBody);
                    emailHelper.SendEmail(result.DoctorEmail, subject, doctorBody);
                }


                if (result.AppointmentStatus.Equals("Pending Patient Confirmation", StringComparison.OrdinalIgnoreCase))
                {
                    var baseUrl = "https://localhost:44398/api/Appointment";
                    var acceptToken = JwtTokenManager.GenerateToken(new UserModel
                    {
                        DoctorID = result.DoctorID,
                        Email = result.PatientEmail,
                        RoleName = "Patient"
                    });

                    var declineToken = JwtTokenManager.GenerateToken(new UserModel
                    {
                        DoctorID = result.DoctorID,
                        Email = result.PatientEmail,
                        RoleName = "Patient"
                    });
                    var acceptUrl = $"{baseUrl}/ConfirmAppointment?token={acceptToken}&statusId=1&appointmentId={result.AppointmentID}";
                    var declineUrl = $"{baseUrl}/ConfirmAppointment?token={declineToken}&statusId=3&appointmentId={result.AppointmentID}";
                    string patientTemplates = "AppointmentPendingConfirmation_Patient.html";
                    string patientBody = emailHelper
                        .LoadEmailTemplate(patientTemplates, result, forDoctor: false)
                        .Replace("{{AcceptUrl}}", acceptUrl)
                        .Replace("{{DeclineUrl}}", declineUrl);

                    string subjects = "Action Required: Please Confirm Your Appointment - MedSync";
                    emailHelper.SendEmail(result.PatientEmail, subjects, patientBody);
                }

                return Ok(new { Success = true, Message = "Appointment updated and emails sent successfully." });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        public IHttpActionResult BookAppointment()
        {
            if (HttpContext.Current.Request == null)
                return BadRequest("Invalid request.");

            try
            {
                var httpRequest = HttpContext.Current.Request;
                var model = new BookAppointmentRequestModel
                {
                    DoctorID = int.Parse(httpRequest.Form["DoctorID"]),
                    AppointmentDate = DateTime.Parse(httpRequest.Form["AppointmentDate"]),
                    StartTime = DateTime.Parse(httpRequest.Form["StartTime"]).TimeOfDay,
                    EndTime = DateTime.Parse(httpRequest.Form["EndTime"]).TimeOfDay,
                    CreatedBy = 1,
                    Patient_FirstName = httpRequest.Form["Patient_FirstName"],
                    Patient_LastName = httpRequest.Form["Patient_LastName"],
                    Patient_GenderID = int.Parse(httpRequest.Form["Patient_GenderID"]),
                    Patient_Email = httpRequest.Form["Patient_Email"],
                    Patient_BloodGroupID = int.TryParse(httpRequest.Form["Patient_BloodGroupID"], out int bgId) ? bgId : (int?)null,
                    Patient_DateOfBirth = DateTime.TryParse(httpRequest.Form["Patient_DateOfBirth"], out DateTime dob) ? dob : (DateTime?)null,
                    Patient_PhoneNumber = httpRequest.Form["Patient_PhoneNumber"],
                    Patient_Address = httpRequest.Form["Patient_Address"],
                    Patient_TalukaID = int.TryParse(httpRequest.Form["Patient_TalukaID"], out int talukaId) ? talukaId : (int?)null,

                    MedicalHistory = httpRequest.Form["Patient_MedicalHistory"],
                    MedicalConcern = httpRequest.Form["Patient_MedicalConcern"],
                    InsuranceDetails = httpRequest.Form["Patient_InsuranceDetails"],

                    PostedFiles = new List<HttpPostedFile>()
                };

                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(model);
                bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errors = validationResults.Select(v => v.ErrorMessage).ToList();
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        success = false,
                        message = "Validation failed.",
                        errors
                    });
                }



                if (httpRequest.Files.Count > 0)
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                        model.PostedFiles.Add(httpRequest.Files[i]);
                }

                var appointmentDal = new Appointment();
                var appointment = appointmentDal.Insert(model);
                if (appointment == null)
                    throw new Exception("Appointment could not be booked or returned from DAL.");


                string patientTemplateFile = "AppointmentRequestSubmitted_Patient.html";
                string doctorTemplateFile = "AppointmentRequestReceived_Doctor.html";

                string patientBody = emailHelper.LoadEmailTemplate(patientTemplateFile, appointment, forDoctor: false);
                string doctorBody = emailHelper.LoadEmailTemplate(doctorTemplateFile, appointment, forDoctor: true);


                emailHelper.SendEmail(appointment.PatientEmail, "Appointment Request Submitted - MedSync", patientBody);
                emailHelper.SendEmail(appointment.DoctorEmail, "New Appointment Request - MedSync", doctorBody);


                return Content(HttpStatusCode.OK, new
                {
                    success = true,
                    message = "Appointment booked successfully. Email notifications sent to both Doctor and Patient.",
                    appointmentId = appointment.AppointmentID
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, 1);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "An error occurred while booking the appointment.",
                    error = ex.Message
                });
            }
        }


        [HttpGet]
        public IHttpActionResult ConfirmAppointment(int appointmentId, int statusId, string token)
        {
            var principal = JwtTokenManager.ValidateToken(token);
            if (principal == null)
                return Unauthorized();

            try
            {
                int doctorId = int.Parse(principal.FindFirst("DoctorID").Value);

                var request = new AppointmentModel
                {
                    AppointmentID = appointmentId,
                    DoctorID = doctorId,
                    AppointmentStatusID = statusId,
                    ModifiedBy = 1
                };

                var appointmentDal = new Appointment();
                var result = appointmentDal.Update(request);
                if (result == null || result.SuccessFlag == 0)
                    return Content(HttpStatusCode.NotFound, "Appointment not found or update failed.");
                if (result.SuccessFlag == -1)
                    return Content(HttpStatusCode.Forbidden, "This appointment can no longer be modified.");

                string subject = $"Appointment {result.AppointmentStatus} - MedSync";

                string patientTemplate = string.Empty;
                string doctorTemplate = string.Empty;

                if (result.AppointmentStatus.Equals("Accepted", StringComparison.OrdinalIgnoreCase))
                {
                    patientTemplate = "RescheduledAppointmentAccepted_Patient.html";
                    doctorTemplate = "RescheduledAppointmentAccepted_Doctor.html";
                }
                else if (result.AppointmentStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                {
                    patientTemplate = "RescheduledAppointmentCancelled_Patient.html";
                    doctorTemplate = "RescheduledAppointmentCancelled_Doctor.html";
                }

                if (!string.IsNullOrEmpty(patientTemplate) && !string.IsNullOrEmpty(doctorTemplate))
                {
                    string patientBody = emailHelper.LoadEmailTemplate(patientTemplate, result, forDoctor: false, 1);
                    string doctorBody = emailHelper.LoadEmailTemplate(doctorTemplate, result, forDoctor: true, 1);
                    emailHelper.SendEmail(result.PatientEmail, subject, patientBody);
                    emailHelper.SendEmail(result.DoctorEmail, subject, doctorBody);
                }
                return Ok(new { Success = true, Message = "Appointment updated and emails sent successfully." });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
                return BadRequest("Failed to update appointment status." + ex.Message);
            }
        }


    }
}




