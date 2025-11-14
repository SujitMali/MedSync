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

        #region GetSlots(int doctorId, DateTime appointmentDate)
        public List<DoctorSlotModel> GetSlots(int doctorId, DateTime appointmentDate)
        {
            var slots = new List<DoctorSlotModel>();
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_DoctorSchedulesGenerateSlots");
                db.AddInParameter(cmd, "@DoctorID", DbType.Int32, doctorId);
                db.AddInParameter(cmd, "@AppointmentDate", DbType.Date, appointmentDate);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0)
                {
                    DateTime dateOnly = appointmentDate.Date;

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TimeSpan startTime = (TimeSpan)row["SlotStart"];
                        TimeSpan endTime = (TimeSpan)row["SlotEnd"];
                        DateTime fullSlotStart = dateOnly.Add(startTime);
                        DateTime fullSlotEnd = dateOnly.Add(endTime);

                        slots.Add(new DoctorSlotModel
                        {
                            SlotStart = fullSlotStart,
                            SlotEnd = fullSlotEnd,
                            IsAvailable = Convert.ToBoolean(row["IsAvailable"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
            }
            return slots;
        }
        #endregion


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
                                    PatientDOB = row["PatientDOB"] == DBNull.Value? (DateTime?)null: Convert.ToDateTime(row["PatientDOB"]),
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
                    db.AddInParameter(cmd, "@AppointmentID", DbType.Int32, request.AppointmentID);
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, request.DoctorID);
                    db.AddInParameter(cmd, "@NewAppointmentStatusID", DbType.Int32, request.AppointmentStatusID);
                    db.AddInParameter(cmd, "@NewAppointmentDate", DbType.Date, request.AppointmentDate == default(DateTime) ? (object)DBNull.Value : request.AppointmentDate);
                    db.AddInParameter(cmd, "@NewStartTime", DbType.String, request.StartTime == default(TimeSpan) ? (object)DBNull.Value : request.StartTime.ToString());
                    db.AddInParameter(cmd, "@NewEndTime", DbType.String, request.EndTime == default(TimeSpan) ? (object)DBNull.Value : request.EndTime.ToString());

                    db.AddInParameter(cmd, "@CancellationReason", DbType.String, string.IsNullOrWhiteSpace(request.CancellationReason) ? (object)DBNull.Value : request.CancellationReason);
                    db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, request.ModifiedBy ?? request.DoctorID);

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
            if (string.IsNullOrEmpty(pathTemplate))
                throw new ConfigurationErrorsException($"The AppSettings key '{appointmentPathKey}' is missing or empty in web.config.");

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
                        db.AddInParameter(cmd, "@Patient_BloodGroupID", DbType.Int32, (object)request.Patient_BloodGroupID ?? DBNull.Value);
                        db.AddInParameter(cmd, "@Patient_DateOfBirth", DbType.Date, (object)request.Patient_DateOfBirth ?? DBNull.Value);
                        db.AddInParameter(cmd, "@Patient_PhoneNumber", DbType.String, (object)request.Patient_PhoneNumber ?? DBNull.Value);
                        db.AddInParameter(cmd, "@Patient_Email", DbType.String, (object)request.Patient_Email ?? DBNull.Value);
                        db.AddInParameter(cmd, "@Patient_Address", DbType.String, (object)request.Patient_Address ?? DBNull.Value);
                        db.AddInParameter(cmd, "@Patient_TalukaID", DbType.Int32, (object)request.Patient_TalukaID ?? DBNull.Value);
                        db.AddInParameter(cmd, "@MedicalHistory", DbType.String, (object)request.MedicalHistory ?? DBNull.Value);
                        db.AddInParameter(cmd, "@InsuranceDetails", DbType.String, (object)request.InsuranceDetails ?? DBNull.Value);
                        db.AddInParameter(cmd, "@MedicalConcern", DbType.String, (object)request.MedicalConcern ?? DBNull.Value);



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
