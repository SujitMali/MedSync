using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using MedSync_ClassLibraries.Models;
using System.Transactions;
using System.Configuration;
using System.Collections.Generic;

namespace MedSync_ClassLibraries.DAL
{
    public class Doctor
    {
        private readonly Database db;

        public Doctor()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<DoctorsModel> GetDoctorsList()
        {
            var doctors = new List<DoctorsModel>();
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_GetDoctorsList");
                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        doctors.Add(new DoctorsModel
                        {
                            DoctorID = Convert.ToInt32(reader["DoctorID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString()
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return doctors;
        }


        public bool Insert(DoctorsModel doc)
        {
            string basePath = ConfigurationManager.AppSettings["ProfilePicturePath"];
            string fullFilePath = null;

            using (var scope = new TransactionScope())
            {
                try
                {
                    // Handle profile picture save if provided
                    if (doc.ProfilePicFile != null && doc.ProfilePicFile.ContentLength > 0)
                    {
                        string originalFileName = Path.GetFileName(doc.ProfilePicFile.FileName);
                        string guid = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(originalFileName);
                        string uniqueFileName = $"{guid}{extension}";
                        fullFilePath = Path.Combine(basePath, uniqueFileName);

                        // Ensure directory exists
                        Directory.CreateDirectory(basePath);

                        // Save file to disk
                        doc.ProfilePicFile.SaveAs(fullFilePath);

                        // Store original name and unique path separately
                        doc.ProfilePicName = originalFileName;
                        doc.ProfilePicPath = uniqueFileName;
                    }


                    // DB insert
                    DbCommand com = db.GetStoredProcCommand("MedSync_DoctorInsert");

                    db.AddOutParameter(com, "DoctorID", DbType.Int32, 1024);
                    db.AddInParameter(com, "FirstName", DbType.String, doc.FirstName);
                    db.AddInParameter(com, "LastName", DbType.String, doc.LastName);
                    db.AddInParameter(com, "Email", DbType.String, doc.Email);
                    db.AddInParameter(com, "PhoneNumber", DbType.String, doc.PhoneNumber);
                    db.AddInParameter(com, "GenderID", DbType.Int32, doc.GenderID);
                    db.AddInParameter(com, "BloodGroupID", DbType.Int32, doc.BloodGroupID);
                    db.AddInParameter(com, "QualificationID", DbType.Int32, doc.QualificationID);
                    db.AddInParameter(com, "CRRIStartDate", DbType.Date, doc.CRRIStartDate ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ConsultationFee", DbType.Decimal, doc.ConsultationFee);
                    db.AddInParameter(com, "DateOfBirth", DbType.Date, doc.DateOfBirth ?? (object)DBNull.Value);
                    db.AddInParameter(com, "Address", DbType.String, doc.Address ?? (object)DBNull.Value);
                    db.AddInParameter(com, "TalukaID", DbType.Int32, doc.TalukaID ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ProfilePicName", DbType.String, doc.ProfilePicName ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ProfilePicPath", DbType.String, doc.ProfilePicPath ?? (object)DBNull.Value);
                    db.AddInParameter(com, "CreatedBy", DbType.Int32, doc.CreatedBy ?? (object)DBNull.Value);

                    db.ExecuteNonQuery(com);
                    doc.DoctorID = Convert.ToInt32(db.GetParameterValue(com, "DoctorID"));

                    scope.Complete();
                    return doc.DoctorID > 0;
                }
                catch (Exception)
                {
                    // Rollback file if DB fails
                    if (File.Exists(fullFilePath))
                        File.Delete(fullFilePath);
                    throw;
                }
            }
        }


        #region GetDoctorsList
        public List<DoctorsModel> GetDoctorsList(DoctorsModel model)
        {
            var doctorList = new List<DoctorsModel>();
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_GetDoctorsList");

                // DoctorID
                if (model.DoctorID == 0)
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, model.DoctorID);

                // Name
                if (string.IsNullOrEmpty(model.FirstName) && string.IsNullOrEmpty(model.LastName))
                    db.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@Name", DbType.String, model.FirstName + " " + model.LastName);

                // GenderID
                if (model.GenderID == 0)
                    db.AddInParameter(cmd, "@GenderID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@GenderID", DbType.Int32, model.GenderID);

                // BloodGroupID
                if (model.BloodGroupID == 0)
                    db.AddInParameter(cmd, "@BloodGroupID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@BloodGroupID", DbType.Int32, model.BloodGroupID);

                // QualificationIDs (comma-separated string)
                if (string.IsNullOrEmpty(model.QualificationIDs))
                    db.AddInParameter(cmd, "@QualificationIDs", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@QualificationIDs", DbType.String, model.QualificationIDs);

                // SpecializationIDs (comma-separated string)
                if (string.IsNullOrEmpty(model.SpecializationIDs))
                    db.AddInParameter(cmd, "@SpecializationIDs", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@SpecializationIDs", DbType.String, model.SpecializationIDs);

                // ConsultationFee
                if (model.ConsultationFee <= 0)
                    db.AddInParameter(cmd, "@ConsultationFee", DbType.Decimal, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@ConsultationFee", DbType.Decimal, model.ConsultationFee);

                // IsActive
                if (model.IsActive == null)
                    db.AddInParameter(cmd, "@IsActive", DbType.Boolean, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@IsActive", DbType.Boolean, model.IsActive);

                // Pagination
                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, model.PageNumber);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, model.PageSize);

                // Sorting
                if (string.IsNullOrEmpty(model.SortColumn))
                    db.AddInParameter(cmd, "@SortColumn", DbType.String, "FirstName");
                else
                    db.AddInParameter(cmd, "@SortColumn", DbType.String, model.SortColumn);

                if (string.IsNullOrEmpty(model.SortDirection))
                    db.AddInParameter(cmd, "@SortDirection", DbType.String, "ASC");
                else
                    db.AddInParameter(cmd, "@SortDirection", DbType.String, model.SortDirection);

                DataSet ds = db.ExecuteDataSet(cmd);

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        doctorList.Add(new DoctorsModel
                        {
                            DoctorID = Convert.ToInt32(row["DoctorID"]),
                            FirstName = row["FirstName"].ToString(),
                            LastName = row["LastName"].ToString(),
                            Email = row["Email"].ToString(),
                            PhoneNumber = row["PhoneNumber"].ToString(),
                            GenderID = Convert.ToInt32(row["GenderID"]),
                            BloodGroupID = Convert.ToInt32(row["BloodGroupID"]),
                            QualificationName = row["QualificationName"].ToString(),
                            ConsultationFee = Convert.ToDecimal(row["ConsultationFee"]),
                            CRRIStartDate = row["CRRIStartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["CRRIStartDate"]) : null,
                            DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["DateOfBirth"]) : null,
                            Address = row["Address"].ToString(),
                            TalukaName = row["TalukaName"].ToString(),
                            ProfilePicName = row["ProfilePicName"].ToString(),
                            ProfilePicPath = row["ProfilePicPath"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            SpecializationName = row["SpecializationName"].ToString()
                        });
                    }

                    // Total records
                    model.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
                }
            }
            catch (Exception ex)
            {
                // Logging can be implemented here
                throw;
            }

            return doctorList;
        }
        #endregion
    }
}
