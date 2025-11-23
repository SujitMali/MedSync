using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MedSync_ClassLibraries.Models;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using System.Configuration;
using MedSync_ClassLibraries.Helpers;


namespace MedSync_ClassLibraries.DAL
{
    public class Appointment
    {
        private readonly Database db;

        public Appointment()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        #region  GetAppointmentsList(AppointmentFilterModel filter)
        public DoctorAppointmentsViewModel GetAppointmentsList(AppointmentFilterModel filter, int CurrentUserId)
        {
            var result = new DoctorAppointmentsViewModel();

            try
            {
                using (DbCommand cmd = db.GetStoredProcCommand("MedSync_AppointmentsGetList"))
                {
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, filter.DoctorID);
                    db.AddInParameter(cmd, "@StatusIDs", DbType.String, filter.StatusIDs);
                    db.AddInParameter(cmd, "@SpecializationIDs", DbType.String, filter.SpecializationIDs);
                    db.AddInParameter(cmd, "@DateFrom", DbType.Date, filter.DateFrom);
                    db.AddInParameter(cmd, "@DateTo", DbType.Date, filter.DateTo);
                    db.AddInParameter(cmd, "@PatientName", DbType.String, filter.PatientName);
                    db.AddInParameter(cmd, "@IsDoctorSuggestedChange", DbType.Boolean, filter.IsDoctorSuggestedChange);
                    db.AddInParameter(cmd, "@PageNumber", DbType.Int32, filter.PageNumber);
                    db.AddInParameter(cmd, "@PageSize", DbType.Int32, filter.PageSize);

                    using (DataSet ds = db.ExecuteDataSet(cmd))
                    {
                        if (ds != null && ds.Tables.Count >= 3)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                result.Appointments.Add(new AppointmentModel
                                {
                                    AppointmentID = Convert.ToInt32(row["AppointmentID"]),
                                    DoctorID = Convert.ToInt32(row["DoctorID"]),
                                    DoctorName = Convert.ToString(row["DoctorName"]),
                                    PatientID = Convert.ToInt32(row["PatientID"]),
                                    PatientName = Convert.ToString(row["PatientName"]),
                                    AppointmentDate = Convert.ToDateTime(row["AppointmentDate"]),
                                    StartTime = (TimeSpan)row["StartTime"],
                                    EndTime = (TimeSpan)row["EndTime"],
                                    AppointmentStatusID = Convert.ToInt32(row["AppointmentStatusID"]),
                                    SpecializationNames = row["SpecializationNames"] == DBNull.Value ? null : Convert.ToString(row["SpecializationNames"]),
                                    StatusName = Convert.ToString(row["StatusName"]),
                                    CancellationReason = row["CancellationReason"] == DBNull.Value ? null : Convert.ToString(row["CancellationReason"]),
                                    IsDoctorSuggestedChange = Convert.ToBoolean(row["IsDoctorSuggestedChange"]),
                                    CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                                    ModifiedOn = row["ModifiedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ModifiedOn"]),
                                    PatientEmail = Convert.ToString(row["PatientEmail"]),
                                    PatientPhone = Convert.ToString(row["PatientPhone"]),
                                    MedicalHistory = row["MedicalHistory"] == DBNull.Value ? null : Convert.ToString(row["MedicalHistory"]),
                                    MedicalConcern = row["MedicalConcern"] == DBNull.Value ? null : Convert.ToString(row["MedicalConcern"]),
                                    InsuranceDetails = row["InsuranceDetails"] == DBNull.Value ? null : Convert.ToString(row["InsuranceDetails"]),
                                    PatientDOB = row["PatientDOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["PatientDOB"]),
                                    PatientGender = row["PatientGender"] == DBNull.Value ? null : Convert.ToString(row["PatientGender"]),
                                    PatientBloodGroup = row["PatientBloodGroup"] == DBNull.Value ? null : Convert.ToString(row["PatientBloodGroup"])
                                });
                            }

                            if (ds.Tables[1].Rows.Count > 0)
                                result.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);

                            foreach (DataRow row in ds.Tables[2].Rows)
                            {
                                result.AppointmentFiles.Add(new AppointmentFileModel
                                {
                                    AppointmentFileID = Convert.ToInt32(row["AppointmentFileID"]),
                                    AppointmentID = Convert.ToInt32(row["AppointmentID"]),
                                    FileName = Convert.ToString(row["FileName"]),
                                    FilePath = Convert.ToString(row["FilePath"]),
                                    UploadedBy = row["UploadedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["UploadedBy"]),
                                    IsActive = Convert.ToBoolean(row["IsActive"]),
                                    CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                                    CreatedBy = row["CreatedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["CreatedBy"]),
                                    ModifiedOn = row["ModifiedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ModifiedOn"]),
                                    ModifiedBy = row["ModifiedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ModifiedBy"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, CurrentUserId);
            }

            return result;
        }
        #endregion


        #region Update(AppointmentModel request)
        public AppointmentUpdateResult Update(AppointmentModel request)
        {
            try
            {
                using (DbCommand cmd = db.GetStoredProcCommand("MedSync_AppointmentsUpdate"))
                {

                    if (request.AppointmentID > 0)
                        db.AddInParameter(cmd, "@AppointmentID", DbType.Int32, request.AppointmentID);
                    else
                        db.AddInParameter(cmd, "@AppointmentID", DbType.Int32, DBNull.Value);

                    if (request.DoctorID > 0)
                        db.AddInParameter(cmd, "@DoctorID", DbType.Int32, request.DoctorID);
                    else
                        db.AddInParameter(cmd, "@DoctorID", DbType.Int32, DBNull.Value);

                    if (request.AppointmentStatusID > 0)
                        db.AddInParameter(cmd, "@NewAppointmentStatusID", DbType.Int32, request.AppointmentStatusID);
                    else
                        db.AddInParameter(cmd, "@NewAppointmentStatusID", DbType.Int32, DBNull.Value);

                    if (request.AppointmentDate == default(DateTime))
                        db.AddInParameter(cmd, "@NewAppointmentDate", DbType.Date, DBNull.Value);
                    else
                        db.AddInParameter(cmd, "@NewAppointmentDate", DbType.Date, request.AppointmentDate);

                    if (request.StartTime == default(TimeSpan))
                        db.AddInParameter(cmd, "@NewStartTime", DbType.String, DBNull.Value);
                    else
                        db.AddInParameter(cmd, "@NewStartTime", DbType.String, request.StartTime.ToString());

                    if (request.EndTime == default(TimeSpan))
                        db.AddInParameter(cmd, "@NewEndTime", DbType.String, DBNull.Value);
                    else
                        db.AddInParameter(cmd, "@NewEndTime", DbType.String, request.EndTime.ToString());


                    if (string.IsNullOrWhiteSpace(request.CancellationReason))
                        db.AddInParameter(cmd, "@CancellationReason", DbType.String, DBNull.Value);
                    else
                        db.AddInParameter(cmd, "@CancellationReason", DbType.String, request.CancellationReason);

                    if (request.ModifiedBy.HasValue)
                        db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, request.ModifiedBy.Value);
                    else
                        db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, request.DoctorID);


                    using (IDataReader reader = db.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            return new AppointmentUpdateResult
                            {
                                SuccessFlag = Convert.ToInt32(reader["SuccessFlag"]),
                                AppointmentID = reader["AppointmentID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AppointmentID"]),
                                DoctorID = reader["DoctorID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DoctorID"]),
                                AppointmentDate = reader["AppointmentDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["AppointmentDate"]),
                                StartTime = reader["StartTime"] == DBNull.Value ? TimeSpan.Zero : (TimeSpan)reader["StartTime"],
                                EndTime = reader["EndTime"] == DBNull.Value ? TimeSpan.Zero : (TimeSpan)reader["EndTime"],
                                AppointmentStatus = reader["AppointmentStatus"]?.ToString(),
                                PatientName = reader["PatientName"]?.ToString(),
                                PatientEmail = reader["PatientEmail"]?.ToString(),
                                DoctorName = reader["DoctorName"]?.ToString(),
                                DoctorEmail = reader["DoctorEmail"]?.ToString(),
                                CancellationReason = reader["CancellationReason"]?.ToString()
                            };
                        }
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, request.CreatedBy);
                return null;
            }
        }
        #endregion


        #region Insert(BookAppointmentRequestModel request)
        public AppointmentModel Insert(BookAppointmentRequestModel request)
        {
            string appointmentPathKey = "AppointmentFilesPath";
            string pathTemplate = ConfigurationManager.AppSettings[appointmentPathKey];

            string finalUploadBasePath = Path.GetDirectoryName(pathTemplate.Replace("{AppointmentId}", ""));
            string uploadRoot = Path.GetDirectoryName(finalUploadBasePath);
            string tempUploadPath = Path.Combine(uploadRoot, "temp");

            var savedTempFiles = new List<string>();
            var finalFilePaths = new List<(string tempPath, string finalPath)>();

            DataTable fileTable = new DataTable();
            fileTable.Columns.Add("FileName", typeof(string));
            fileTable.Columns.Add("FilePath", typeof(string));
            AppointmentModel appointment = null;

            using (var scope = new TransactionScope())
            {
                try
                {
                    Directory.CreateDirectory(tempUploadPath);

                    if (request.PostedFiles != null && request.PostedFiles.Count > 0)
                    {
                        foreach (var file in request.PostedFiles)
                        {
                            string originalFileName = Path.GetFileName(file.FileName);
                            string extension = Path.GetExtension(originalFileName);
                            string guidFileName = $"{Guid.NewGuid()}{extension}";
                            string tempFilePath = Path.Combine(tempUploadPath, guidFileName);

                            file.SaveAs(tempFilePath);
                            savedTempFiles.Add(tempFilePath);
                            fileTable.Rows.Add(originalFileName, guidFileName);
                        }
                    }

                    using (DbCommand cmd = db.GetStoredProcCommand("MedSync_AppointmentsInsert"))
                    {


                        DateTime appointmentDate = request.AppointmentDate.Date;
                        DateTime fullStartTime = appointmentDate.Add(request.StartTime);
                        DateTime fullEndTime = appointmentDate.Add(request.EndTime);

                        db.AddInParameter(cmd, "@DoctorID", DbType.Int32, request.DoctorID);
                        db.AddInParameter(cmd, "@AppointmentDate", DbType.Date, request.AppointmentDate);
                        db.AddInParameter(cmd, "@StartTime", DbType.DateTime, fullStartTime);
                        db.AddInParameter(cmd, "@EndTime", DbType.DateTime, fullEndTime);
                        db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, request.CreatedBy);
                        db.AddInParameter(cmd, "@Patient_FirstName", DbType.String, request.Patient_FirstName);
                        db.AddInParameter(cmd, "@Patient_LastName", DbType.String, request.Patient_LastName);
                        db.AddInParameter(cmd, "@Patient_GenderID", DbType.Int32, request.Patient_GenderID);

                        if (request.Patient_BloodGroupID.HasValue)
                            db.AddInParameter(cmd, "@Patient_BloodGroupID", DbType.Int32, request.Patient_BloodGroupID.Value);
                        else
                            db.AddInParameter(cmd, "@Patient_BloodGroupID", DbType.Int32, DBNull.Value);


                        if (request.Patient_DateOfBirth.HasValue)
                            db.AddInParameter(cmd, "@Patient_DateOfBirth", DbType.Date, request.Patient_DateOfBirth.Value);
                        else
                            db.AddInParameter(cmd, "@Patient_DateOfBirth", DbType.Date, DBNull.Value);


                        if (!string.IsNullOrWhiteSpace(request.Patient_PhoneNumber))
                            db.AddInParameter(cmd, "@Patient_PhoneNumber", DbType.String, request.Patient_PhoneNumber);
                        else
                            db.AddInParameter(cmd, "@Patient_PhoneNumber", DbType.String, DBNull.Value);


                        if (!string.IsNullOrWhiteSpace(request.Patient_Email))
                            db.AddInParameter(cmd, "@Patient_Email", DbType.String, request.Patient_Email);
                        else
                            db.AddInParameter(cmd, "@Patient_Email", DbType.String, DBNull.Value);


                        if (!string.IsNullOrWhiteSpace(request.Patient_Address))
                            db.AddInParameter(cmd, "@Patient_Address", DbType.String, request.Patient_Address);
                        else
                            db.AddInParameter(cmd, "@Patient_Address", DbType.String, DBNull.Value);


                        if (request.Patient_TalukaID.HasValue)
                            db.AddInParameter(cmd, "@Patient_TalukaID", DbType.Int32, request.Patient_TalukaID.Value);
                        else
                            db.AddInParameter(cmd, "@Patient_TalukaID", DbType.Int32, DBNull.Value);


                        if (!string.IsNullOrWhiteSpace(request.MedicalHistory))
                            db.AddInParameter(cmd, "@MedicalHistory", DbType.String, request.MedicalHistory);
                        else
                            db.AddInParameter(cmd, "@MedicalHistory", DbType.String, DBNull.Value);


                        if (!string.IsNullOrWhiteSpace(request.InsuranceDetails))
                            db.AddInParameter(cmd, "@InsuranceDetails", DbType.String, request.InsuranceDetails);
                        else
                            db.AddInParameter(cmd, "@InsuranceDetails", DbType.String, DBNull.Value);


                        if (!string.IsNullOrWhiteSpace(request.MedicalConcern))
                            db.AddInParameter(cmd, "@MedicalConcern", DbType.String, request.MedicalConcern);
                        else
                            db.AddInParameter(cmd, "@MedicalConcern", DbType.String, DBNull.Value);


                        SqlParameter sqlParam = new SqlParameter("@Files", SqlDbType.Structured)
                        {
                            TypeName = "MedSync_AppointmentFileTVP",
                            Value = fileTable
                        };
                        ((SqlCommand)cmd).Parameters.Add(sqlParam);

                        using (IDataReader reader = db.ExecuteReader(cmd))
                        {
                            if (reader.Read())
                            {
                                appointment = new AppointmentModel
                                {
                                    AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                                    DoctorID = Convert.ToInt32(reader["DoctorID"]),
                                    DoctorName = reader["DoctorName"].ToString(),
                                    DoctorEmail = reader["DoctorEmail"].ToString(),
                                    PatientID = Convert.ToInt32(reader["PatientID"]),
                                    PatientName = reader["PatientName"].ToString(),
                                    PatientEmail = reader["PatientEmail"].ToString(),
                                    AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                                    StartTime = reader["StartTime"] == DBNull.Value ? TimeSpan.Zero : (TimeSpan)reader["StartTime"],
                                    EndTime = reader["EndTime"] == DBNull.Value ? TimeSpan.Zero : (TimeSpan)reader["EndTime"]
                                };
                            }
                        }
                    }

                    if (fileTable.Rows.Count > 0)
                    {
                        string finalAppointmentPath = Path.Combine(finalUploadBasePath, appointment.AppointmentID.ToString());
                        Directory.CreateDirectory(finalAppointmentPath);

                        foreach (DataRow row in fileTable.Rows)
                        {
                            string guidFileName = row["FilePath"].ToString();
                            string tempFilePath = Path.Combine(tempUploadPath, guidFileName);
                            string finalFilePath = Path.Combine(finalAppointmentPath, guidFileName);

                            File.Move(tempFilePath, finalFilePath);
                            savedTempFiles.Remove(tempFilePath);
                            finalFilePaths.Add((tempFilePath, finalFilePath));
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    foreach (var path in finalFilePaths)
                        if (File.Exists(path.finalPath)) File.Delete(path.finalPath);

                    foreach (var tempPath in savedTempFiles)
                        if (File.Exists(tempPath)) File.Delete(tempPath);

                    DbErrorLogger.LogError(ex, request.CreatedBy);
                }
                return appointment;
            }
        }
        #endregion


    }
}