using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class DoctorSchedule
    {
        private readonly Database db;

        public DoctorSchedule()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        #region GetSchedulesListByDoctorId(int doctorID, int CurrentUserId)
        public List<DoctorScheduleModel> GetSchedulesListByDoctorId(int doctorID, int CurrentUserId)
        {
            var schedules = new List<DoctorScheduleModel>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_DoctorsSchedulesGetListByDoctorId");
                db.AddInParameter(cmd, "@DoctorID", DbType.Int32, doctorID);

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        schedules.Add(new DoctorScheduleModel
                        {
                            DoctorScheduleID = Convert.ToInt32(reader["DoctorScheduleID"]),
                            DoctorID = Convert.ToInt32(reader["DoctorID"]),
                            DayOfWeek = Convert.ToByte(reader["DayOfWeek"]),
                            StartTime = (TimeSpan)reader["StartTime"],
                            EndTime = (TimeSpan)reader["EndTime"],
                            SlotDurationID = Convert.ToInt32(reader["SlotDurationID"]),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: CurrentUserId);
            }

            return schedules;
        }
        #endregion


        #region Upsert(int doctorId, List<DoctorScheduleModel> schedules, int userId = 1)
        public bool Upsert(int doctorId, List<DoctorScheduleModel> schedules, int userId = 1)
        {
            try
            {
                if (schedules == null)
                    throw new ArgumentNullException(nameof(schedules), "Schedules list cannot be null.");

                DataTable tvp = new DataTable();
                tvp.Columns.Add("DoctorScheduleID", typeof(int));
                tvp.Columns.Add("DoctorID", typeof(int));
                tvp.Columns.Add("DayOfWeek", typeof(byte));
                tvp.Columns.Add("StartTime", typeof(TimeSpan));
                tvp.Columns.Add("EndTime", typeof(TimeSpan));
                tvp.Columns.Add("SlotDurationID", typeof(int));
                tvp.Columns.Add("IsActive", typeof(bool));

                foreach (var s in schedules)
                {
                    var row = tvp.NewRow();
                    row["DoctorScheduleID"] = s.DoctorScheduleID.HasValue ? (object)s.DoctorScheduleID.Value : DBNull.Value;
                    row["DoctorID"] = s.DoctorID;
                    row["DayOfWeek"] = s.DayOfWeek;
                    row["StartTime"] = s.StartTime;
                    row["EndTime"] = s.EndTime;
                    row["SlotDurationID"] = s.SlotDurationID;
                    row["IsActive"] = s.IsActive;
                    tvp.Rows.Add(row);
                }

                DbCommand cmd = db.GetStoredProcCommand("MedSync_DoctorSchedulesUpsert");
                db.AddInParameter(cmd, "@DoctorID", DbType.Int32, doctorId);
                db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);

                var sqlCmd = cmd as System.Data.SqlClient.SqlCommand;
                if (sqlCmd == null)
                {
                    throw new InvalidOperationException("Underlying command is not a SqlCommand. TVPs require SQL Server.");
                }

                var tvpParam = sqlCmd.Parameters.AddWithValue("@Schedules", tvp);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "dbo.MedSync_DoctorSchedules_TVP";

                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: userId);
                //return false;
                throw;
            }
        }
        #endregion
   
    
    }
}

