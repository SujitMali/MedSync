using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MedSync_ClassLibraries.DAL
{
    public class DoctorSchedule
    {
        private readonly Database db;

        public DoctorSchedule()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<DoctorScheduleModel> GetSchedulesByDoctor(int doctorID)
        {
            var schedules = new List<DoctorScheduleModel>();
            DbCommand cmd = db.GetStoredProcCommand("MedSync_GetDoctorSchedules");
            db.AddInParameter(cmd, "DoctorID", DbType.Int32, doctorID);

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

            return schedules;
        }



        //public bool SaveSchedules_TVP(int doctorId, List<DoctorScheduleModel> schedules, int userId = 1)
        //{
        //    if (schedules == null) throw new ArgumentNullException(nameof(schedules));

        //    // Build DataTable matching TVP type
        //    DataTable tvp = new DataTable();
        //    tvp.Columns.Add("DoctorScheduleID", typeof(int));
        //    tvp.Columns.Add("DoctorID", typeof(int));
        //    tvp.Columns.Add("DayOfWeek", typeof(byte));
        //    tvp.Columns.Add("StartTime", typeof(TimeSpan));
        //    tvp.Columns.Add("EndTime", typeof(TimeSpan));
        //    tvp.Columns.Add("SlotDurationID", typeof(int));
        //    tvp.Columns.Add("IsActive", typeof(bool));

        //    foreach (var s in schedules)
        //    {
        //        var row = tvp.NewRow();
        //        row["DoctorScheduleID"] = s.DoctorScheduleID.HasValue ? (object)s.DoctorScheduleID.Value : DBNull.Value;
        //        row["DoctorID"] = s.DoctorID;
        //        row["DayOfWeek"] = s.DayOfWeek;
        //        row["StartTime"] = s.StartTime;
        //        row["EndTime"] = s.EndTime;
        //        row["SlotDurationID"] = s.SlotDurationID;
        //        row["IsActive"] = s.IsActive;
        //        tvp.Rows.Add(row);
        //    }

        //    // Use connection string from config (same DB as your EnterpriseLibrary)
        //    var connString = ConfigurationManager.ConnectionStrings["YourConnectionStringName"].ConnectionString;

        //    using (var conn = new System.Data.SqlClient.SqlConnection(connString))
        //    using (var cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "MedSync_UpsertDoctorSchedules";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        var p1 = cmd.Parameters.AddWithValue("@DoctorID", doctorId);
        //        p1.SqlDbType = SqlDbType.Int;

        //        var p2 = cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter());
        //        p2.ParameterName = "@Schedules";
        //        p2.SqlDbType = SqlDbType.Structured;
        //        p2.TypeName = "dbo.MedSync_DoctorSchedules_TVP";
        //        p2.Value = tvp;
        //        cmd.Parameters.Add(p2);

        //        var p3 = cmd.Parameters.AddWithValue("@UserID", userId);
        //        p3.SqlDbType = SqlDbType.Int;

        //        conn.Open();
        //        cmd.CommandTimeout = 120; // adjust if needed
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }
        //}


        public bool SaveSchedules_TVP(int doctorId, List<DoctorScheduleModel> schedules, int userId = 1)
        {
            if (schedules == null)
                throw new ArgumentNullException(nameof(schedules));

            // Build DataTable for TVP
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

            // Use Enterprise Library to execute the stored procedure
            DbCommand cmd = db.GetStoredProcCommand("MedSync_UpsertDoctorSchedules");

            // Add parameters using Enterprise Library
            db.AddInParameter(cmd, "@DoctorID", DbType.Int32, doctorId);
            db.AddInParameter(cmd, "@UserID", DbType.Int32, userId);

            // For Table-Valued Parameter, we need to use the underlying SqlCommand object
            // Enterprise Library does not directly support TVPs, but we can cast DbCommand
            var sqlCmd = cmd as System.Data.SqlClient.SqlCommand;
            if (sqlCmd == null)
            {
                throw new InvalidOperationException("Underlying command is not a SqlCommand. TVPs require SQL Server.");
            }

            var tvpParam = sqlCmd.Parameters.AddWithValue("@Schedules", tvp);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.MedSync_DoctorSchedules_TVP";

            // Execute command
            db.ExecuteNonQuery(cmd);
            return true;
        }

    }
}



//public bool SaveSchedules(List<DoctorScheduleModel> schedules)
//{
//    using (var scope = new System.Transactions.TransactionScope())
//    {
//        try
//        {
//            foreach (var s in schedules)
//            {
//                DbCommand cmd = db.GetStoredProcCommand("MedSync_UpsertDoctorSchedule");

//                db.AddInParameter(cmd, "DoctorScheduleID", DbType.Int32, s.DoctorScheduleID);
//                db.AddInParameter(cmd, "DoctorID", DbType.Int32, s.DoctorID);
//                db.AddInParameter(cmd, "DayOfWeek", DbType.Byte, s.DayOfWeek);
//                db.AddInParameter(cmd, "StartTime", DbType.Time, s.StartTime);
//                db.AddInParameter(cmd, "EndTime", DbType.Time, s.EndTime);
//                db.AddInParameter(cmd, "SlotDurationID", DbType.Int32, s.SlotDurationID);
//                db.AddInParameter(cmd, "IsActive", DbType.Boolean, s.IsActive);
//                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, s.CreatedBy ?? 1);

//                db.ExecuteNonQuery(cmd);
//            }

//            scope.Complete();
//            return true;
//        }
//        catch
//        {
//            throw;
//        }
//    }
//}

// using System.Data.SqlClient; (make sure to add)
// using System.Configuration;