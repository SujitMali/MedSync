//using System;
//using System.Net;
//using System.Web.Http;
//using MedSync_ClassLibraries.DAL;
//using MedSync_ClassLibraries.Models;
//using System.Web;
//using System.Collections.Generic;
//using System.Linq;

//namespace MedSync_API.Controllers
//{
//    [RoutePrefix("api/admin")]
//    public class AdminController : ApiController
//    {
//        [HttpGet]
//        [Route("get-dropdown-data")]
//        public IHttpActionResult GetDropdownData()
//        {
//            try
//            {
//                var genderDal = new Gender();
//                var bloodGroupDal = new BloodGroup();
//                var qualificationDal = new Qualification();
//                var talukaDal = new Taluka();
//                var districtDal = new District();
//                var stateDal = new State();

//                var genders = genderDal.GetAllGenders();
//                var bloodGroups = bloodGroupDal.GetAllBloodGroups();
//                var qualifications = qualificationDal.GetAllQualifications();
//                var talukas = talukaDal.GetTalukaByDistrict(null);
//                var districts = districtDal.GetDistrictsByState(null);
//                var states = stateDal.GetAllStates();


//                var result = new
//                {
//                    success = true,
//                    message = "Dropdown data fetched successfully.",
//                    data = new
//                    {
//                        genders,
//                        bloodGroups,
//                        qualifications,
//                        talukas,
//                        districts,
//                        states
//                    }
//                };

//                return Ok(result);
//            }
//            catch (Exception ex)
//            {
//                return Content(HttpStatusCode.InternalServerError, new
//                {
//                    success = false,
//                    message = $"An error occurred while fetching dropdown data: {ex.Message}"
//                });
//            }
//        }


//        [HttpPost]
//        [Route("add-doctor")]
//        public IHttpActionResult AddDoctor()
//        {
//            try
//            {
//                var httpRequest = HttpContext.Current.Request;

//                if (httpRequest.Form.Count == 0)
//                    return BadRequest("No form data received. Please use multipart/form-data.");


//                DoctorsModel doctor = new DoctorsModel
//                {
//                    FirstName = httpRequest.Form["FirstName"],
//                    LastName = httpRequest.Form["LastName"],
//                    Email = httpRequest.Form["Email"],
//                    PhoneNumber = httpRequest.Form["PhoneNumber"],
//                    GenderID = Convert.ToInt32(httpRequest.Form["GenderID"]),
//                    BloodGroupID = Convert.ToInt32(httpRequest.Form["BloodGroupID"]),
//                    QualificationID = Convert.ToInt32(httpRequest.Form["QualificationID"]),
//                    CRRIStartDate = string.IsNullOrEmpty(httpRequest.Form["CRRIStartDate"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["CRRIStartDate"]),
//                    ConsultationFee = string.IsNullOrEmpty(httpRequest.Form["ConsultationFee"]) ? 0 : Convert.ToDecimal(httpRequest.Form["ConsultationFee"]),
//                    DateOfBirth = string.IsNullOrEmpty(httpRequest.Form["DateOfBirth"]) ? (DateTime?)null : Convert.ToDateTime(httpRequest.Form["DateOfBirth"]),
//                    Address = httpRequest.Form["Address"],
//                    TalukaID = string.IsNullOrEmpty(httpRequest.Form["TalukaID"]) ? (int?)null : Convert.ToInt32(httpRequest.Form["TalukaID"]),
//                    CreatedBy = 1
//                };

//                if (httpRequest.Files.Count > 0)
//                {
//                    doctor.ProfilePicFile = httpRequest.Files["ProfilePicFile"];
//                }

//                var dal = new Doctor();
//                bool isInserted = dal.Insert(doctor);

//                if (isInserted)
//                {
//                    return Ok(new
//                    {
//                        success = true,
//                        message = "Doctor added successfully.",
//                        doctorID = doctor.DoctorID
//                    });
//                }
//                else
//                {
//                    return Content(HttpStatusCode.InternalServerError, new
//                    {
//                        success = false,
//                        message = "Failed to add doctor. Please try again later."
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                return Content(HttpStatusCode.InternalServerError, new
//                {
//                    success = false,
//                    message = $"Error while adding doctor: {ex.Message}"
//                });
//            }
//        }


//        [HttpGet]
//        [Route("get-doctors-list")]
//        public IHttpActionResult GetDoctorsList()
//        {
//            try
//            {
//                var dal = new Doctor();
//                var doctors = dal.GetDoctorsList();

//                return Ok(new
//                {
//                    success = true,
//                    message = "Doctors fetched successfully.",
//                    data = doctors
//                });
//            }
//            catch (Exception ex)
//            {
//                return Content(HttpStatusCode.InternalServerError, new
//                {
//                    success = false,
//                    message = $"Error fetching doctors: {ex.Message}"
//                });
//            }
//        }


//        [HttpGet]
//        [Route("get-doctor-schedule/{doctorID:int}")]
//        public IHttpActionResult GetDoctorSchedule(int doctorID)
//        {
//            try
//            {
//                var dal = new DoctorSchedule();
//                var schedules = dal.GetSchedulesByDoctor(doctorID);
//                return Ok(schedules);
//            }
//            catch (Exception ex)
//            {
//                return Content(HttpStatusCode.InternalServerError, new
//                {
//                    success = false,
//                    message = $"Error fetching schedules: {ex.Message}"
//                });
//            }
//        }


//        [HttpGet]
//        [Route("get-slot-durations")]
//        public IHttpActionResult GetSlotDurations()
//        {
//            try
//            {
//                var dal = new SlotDuration();
//                var durations = dal.GetAllSlotDurations();

//                return Ok(new
//                {
//                    success = true,
//                    message = "Slot durations fetched successfully.",
//                    data = durations
//                });
//            }
//            catch (Exception ex)
//            {
//                return Content(HttpStatusCode.InternalServerError, new
//                {
//                    success = false,
//                    message = $"Error fetching slot durations: {ex.Message}"
//                });
//            }
//        }



//        [HttpPost]
//        [Route("save-doctor-schedules")]
//        public IHttpActionResult SaveDoctorSchedules([FromBody] List<DoctorScheduleModel> schedules)
//        {
//            if (schedules == null || schedules.Count == 0)
//                return BadRequest("No schedules provided.");

//            try
//            {
//                int doctorId = schedules.First().DoctorID; // assume all rows are for same doctor
//                int userId = 1; // derive from auth in real app

//                var dal = new DoctorSchedule();
//                bool result = dal.SaveSchedules_TVP(doctorId, schedules, userId);

//                return Ok(new
//                {
//                    success = result,
//                    message = result ? "Schedules saved successfully." : "Failed to save schedules."
//                });
//            }
//            catch (Exception ex)
//            {
//                return Content(HttpStatusCode.InternalServerError, new
//                {
//                    success = false,
//                    message = $"Error saving schedules: {ex.Message}"
//                });
//            }
//        }


//    }

//}


using System;
using System.Net;
using System.Web.Http;
using MedSync_ClassLibraries.DAL;
using MedSync_ClassLibraries.Models;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace MedSync_API.Controllers
{
    [RoutePrefix("api/admin")]
    public class AdminController : ApiController
    {
        // -------------------------------------------------------------
        // 1. DROPDOWN DATA
        // -------------------------------------------------------------
        [HttpGet]
        [Route("get-dropdown-data")]
        public IHttpActionResult GetDropdownData()
        {
            try
            {
                var genderDal = new Gender();
                var bloodGroupDal = new BloodGroup();
                var qualificationDal = new Qualification();
                var talukaDal = new Taluka();
                var districtDal = new District();
                var stateDal = new State();

                var genders = genderDal.GetAllGenders();
                var bloodGroups = bloodGroupDal.GetAllBloodGroups();
                var qualifications = qualificationDal.GetAllQualifications();
                var talukas = talukaDal.GetTalukaByDistrict(null);
                var districts = districtDal.GetDistrictsByState(null);
                var states = stateDal.GetAllStates();

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
                        states
                    }
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"An error occurred while fetching dropdown data: {ex.Message}",
                    data = (object)null
                });
            }
        }

        // -------------------------------------------------------------
        // 2. ADD DOCTOR
        // -------------------------------------------------------------
        [HttpPost]
        [Route("add-doctor")]
        public IHttpActionResult AddDoctor()
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

                DoctorsModel doctor = new DoctorsModel
                {
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
                    CreatedBy = 1
                };

                if (httpRequest.Files.Count > 0)
                {
                    doctor.ProfilePicFile = httpRequest.Files["ProfilePicFile"];
                }

                var dal = new Doctor();
                bool isInserted = dal.Insert(doctor);

                if (isInserted)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Doctor added successfully.",
                        data = new { doctorID = doctor.DoctorID }
                    });
                }

                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = "Failed to add doctor. Please try again later.",
                    data = (object)null
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error while adding doctor: {ex.Message}",
                    data = (object)null
                });
            }
        }

        // -------------------------------------------------------------
        // 3. GET DOCTORS LIST
        // -------------------------------------------------------------
        [HttpGet]
        [Route("get-doctors-list")]
        public IHttpActionResult GetDoctorsList()
        {
            try
            {
                var dal = new Doctor();
                var doctors = dal.GetDoctorsList();

                return Ok(new
                {
                    success = true,
                    message = "Doctors fetched successfully.",
                    data = doctors
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching doctors: {ex.Message}",
                    data = (object)null
                });
            }
        }

        // -------------------------------------------------------------
        // 4. GET DOCTOR SCHEDULE
        // -------------------------------------------------------------
        [HttpGet]
        [Route("get-doctor-schedule/{doctorID:int}")]
        public IHttpActionResult GetDoctorSchedule(int doctorID)
        {
            try
            {
                var dal = new DoctorSchedule();
                var schedules = dal.GetSchedulesByDoctor(doctorID);

                return Ok(new
                {
                    success = true,
                    message = "Doctor schedules fetched successfully.",
                    data = schedules
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching schedules: {ex.Message}",
                    data = (object)null
                });
            }
        }

        // -------------------------------------------------------------
        // 5. GET SLOT DURATIONS
        // -------------------------------------------------------------
        [HttpGet]
        [Route("get-slot-durations")]
        public IHttpActionResult GetSlotDurations()
        {
            try
            {
                var dal = new SlotDuration();
                var durations = dal.GetAllSlotDurations();

                return Ok(new
                {
                    success = true,
                    message = "Slot durations fetched successfully.",
                    data = durations
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching slot durations: {ex.Message}",
                    data = (object)null
                });
            }
        }

        // -------------------------------------------------------------
        // 6. SAVE DOCTOR SCHEDULES
        // -------------------------------------------------------------
        [HttpPost]
        [Route("save-doctor-schedules")]
        public IHttpActionResult SaveDoctorSchedules([FromBody] List<DoctorScheduleModel> schedules)
        {
            if (schedules == null || schedules.Count == 0)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = "No schedules provided.",
                    data = (object)null
                });
            }

            try
            {
                int doctorId = schedules.First().DoctorID; // assume all for same doctor
                int userId = 1; // placeholder for authenticated user ID

                var dal = new DoctorSchedule();
                bool result = dal.SaveSchedules_TVP(doctorId, schedules, userId);

                return Ok(new
                {
                    success = result,
                    message = result ? "Schedules saved successfully." : "Failed to save schedules.",
                    data = new { doctorID = doctorId }
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error saving schedules: {ex.Message}",
                    data = (object)null
                });
            }
        }
    }
}
