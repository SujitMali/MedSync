using MedSync_ClassLibraries.Models;
using System.Net;
using System;
using System.Web.Http;
using MedSync_ClassLibraries.DAL;
using MedSync_API.Filter;
using MedSync_ClassLibraries.Helpers;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace MedSync_API.Controllers
{

    public class DoctorController : BaseApiController
    {


        [HttpPost]
        public IHttpActionResult GetAllDoctorsList([FromBody] DoctorsModel model)
        {
            try
            {
                var dal = new Doctor();

                if (model.DoctorID > 0)
                {
                    var doctor = dal.GetDoctorsList(model);
                    if (doctor == null)
                        return Ok(new { success = false, message = "Doctor not found.", data = (object)null });

                    return Ok(new
                    {
                        success = true,
                        message = "Doctor fetched successfully.",
                        data = doctor
                    });
                }

                var doctors = dal.GetDoctorsList(model);

                return Ok(new
                {
                    success = true,
                    message = "Doctors fetched successfully.",
                    data = doctors,
                    totalRecords = model.TotalRecords
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching doctors: {ex.Message}",
                    data = (object)null
                });
            }
        }


        [JwtAuthenticate]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult AddEditDoctor()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Form.Count == 0)
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        success = false,
                        message = "No form data received. Please use multipart/form-data.",
                        data = (object)null
                    });
                }

                var model = new DoctorsModel
                {
                    DoctorID = string.IsNullOrEmpty(httpRequest.Form["DoctorID"]) ? 0 : Convert.ToInt32(httpRequest.Form["DoctorID"]),
                    FirstName = httpRequest.Form["FirstName"],
                    LastName = httpRequest.Form["LastName"],
                    Email = httpRequest.Form["Email"],
                    PhoneNumber = httpRequest.Form["PhoneNumber"],
                    GenderID = Convert.ToInt32(httpRequest.Form["GenderID"]),
                    BloodGroupID = Convert.ToInt32(httpRequest.Form["BloodGroupID"]),
                    QualificationID = Convert.ToInt32(httpRequest.Form["QualificationID"]),
                    CRRIStartDate = string.IsNullOrEmpty(httpRequest.Form["CRRIStartDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["CRRIStartDate"]),
                    ConsultationFee = string.IsNullOrEmpty(httpRequest.Form["ConsultationFee"]) ? 0 : Convert.ToDecimal(httpRequest.Form["ConsultationFee"]),
                    DateOfBirth = string.IsNullOrEmpty(httpRequest.Form["DateOfBirth"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["DateOfBirth"]),
                    Address = httpRequest.Form["Address"],
                    TalukaID = string.IsNullOrEmpty(httpRequest.Form["TalukaID"]) ? (int?)null : Convert.ToInt32(httpRequest.Form["TalukaID"]),
                    CreatedBy = CurrentUserId,
                    SpecializationIDs = httpRequest.Form["SpecializationIDs"]
                };

                if (httpRequest.Files.Count > 0)
                {
                    model.ProfilePicFile = httpRequest.Files["ProfilePicFile"];
                }

                var validationErrors = MedSync_API.Validators.DoctorValidator.ValidateDoctorModel(model);

                if (validationErrors.Any())
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        success = false,
                        message = "Validation failed. Please correct the following issues:",
                        data = validationErrors
                    });
                }



                var doctorsDal = new Doctor();
                bool isSuccess;

                if (model.DoctorID > 0)
                    model.ModifiedBy = CurrentUserId;
          
                isSuccess = doctorsDal.Upsert(model);

                if (isSuccess)
                {
                    return Ok(new
                    {
                        success = true,
                        message = model.DoctorID > 0 ? "Doctor updated successfully." : "Doctor added successfully.",
                        data = new { doctorID = model.DoctorID }
                    });
                }

                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "Failed to save doctor record.",
                    data = (object)null
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error while saving doctor: {ex.Message}",
                    data = (object)null
                });
            }
        }


        [JwtAuthenticate]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IHttpActionResult GetDoctorsWithoutUserCredentials()
        {
            try
            {
                var objUsersDal = new User();
                var doctors = objUsersDal.GetDoctorsWithoutCredentials();

                if (doctors == null || doctors.Count == 0)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "All doctors already have active user credentials.",
                        data = new List<DoctorsModel>()
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Doctors without active user credentials fetched successfully.",
                    data = doctors
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching doctors without active users: {ex.Message}",
                    data = (object)null
                });
            }
        }


        [JwtAuthenticate]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult AddUserCredentials([FromBody] UserModel model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Invalid request data.");

                var usersDal = new User();
                model.CreatedBy = CurrentUserId;
                var result = usersDal.Insert(model);

                if (result)
                {
                    return Ok(new { success = true, message = "Doctor credentials added successfully." });
                }

                return Ok(new { success = false, message = "Failed to add doctor credentials." });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error adding doctor credentials: {ex.Message}"
                });
            }
        }


        [HttpGet]
        public IHttpActionResult GetDoctorSlots(int doctorId, DateTime appointmentDate)
        {
            try
            {
                DoctorSchedule doctorScheduleDal = new DoctorSchedule();
                var slots = doctorScheduleDal.GetSlots(doctorId, appointmentDate);
                return Ok(new { success = true, data = slots });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public IHttpActionResult GetDropdownData()
        {
            try
            {
                var genderDal = new Gender();
                var bloodGroupDal = new BloodGroup();
                var qualificationDal = new Qualification();
                var specilizationDal = new Specialization();
                var talukaDal = new Taluka();
                var districtDal = new District();
                var stateDal = new State();

                var genders = genderDal.GetGendersList();
                var bloodGroups = bloodGroupDal.GetBloodGroupList();
                var qualifications = qualificationDal.GetQualificationsList();
                var talukas = talukaDal.GetTalukaListByDistrictId(null);
                var districts = districtDal.GetDistrictListByStateId(null);
                var states = stateDal.GetStateList();
                var specializations = specilizationDal.GetSpecializationList();

                return Ok(new
                {
                    success = true,
                    message = "Dropdown data fetched successfully.",
                    data = new
                    {
                        genders,
                        bloodGroups,
                        qualifications,
                        talukas,
                        districts,
                        states,
                        specializations,
                        feeRange = new { min = 100, max = 5000 }
                    }
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"An error occurred while fetching dropdown data: {ex.Message}",
                    data = (object)null
                });
            }
        }

    }
}
