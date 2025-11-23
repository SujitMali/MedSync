using MedSync_API.Filter;
using MedSync_ClassLibraries.DAL;
using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MedSync_API.Controllers
{
    public class ScheduleController : BaseApiController
    {


        [HttpGet]
        [JwtAuthenticate]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetDoctorSchedule(int doctorID)
        {
            try
            {
                var doctorSchedulesDal = new DoctorSchedule();
                var schedules = doctorSchedulesDal.GetSchedulesListByDoctorId(doctorID, CurrentUserId);

                return Ok(new
                {
                    success = true,
                    message = "Doctor schedules fetched successfully.",
                    data = schedules
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching schedules: {ex.Message}",
                    data = (object)null
                });
            }
        }




        [HttpPost]
        [JwtAuthenticate]
        [Authorize(Roles = "Admin")]
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
                int doctorId = schedules.First().DoctorID;

                var doctorScheduleDal = new DoctorSchedule();
                bool result = doctorScheduleDal.Upsert(doctorId, schedules, CurrentUserId);

                return Ok(new
                {
                    success = result,
                    message = result ? "Schedules saved successfully." : "Failed to save schedules.",
                    data = new { doctorID = doctorId }
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error saving schedules: {ex.Message}",
                    data = (object)null
                });
            }
        }





        [HttpGet]
        [JwtAuthenticate]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetSlotDurations()
        {
            try
            {
                var slotDurationDal = new SlotDuration();
                var durations = slotDurationDal.GetSlotDurationList(CurrentUserId);

                return Ok(new
                {
                    success = true,
                    message = "Slot durations fetched successfully.",
                    data = durations
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: CurrentUserId);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Error fetching slot durations: {ex.Message}",
                    data = (object)null
                });
            }
        }


    }
}
