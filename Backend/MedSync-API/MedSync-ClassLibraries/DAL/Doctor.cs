using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.IO;
using MedSync_ClassLibraries.Models;
using System.Transactions;
using System.Configuration;
using System.Collections.Generic;
using MedSync_ClassLibraries.Helpers;

namespace MedSync_ClassLibraries.DAL
{
    public class Doctor
    {

        private readonly Database db;

        public Doctor()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        #region GetDoctorsList(DoctorsModel model)
        public List<DoctorsModel> GetDoctorsList(DoctorsModel model)
        {
            var doctorList = new List<DoctorsModel>();
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_DoctorsGetList");

                if (model.DoctorID == 0)
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, model.DoctorID);

                if (string.IsNullOrEmpty(model.Name))
                    db.AddInParameter(cmd, "@Name", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@Name", DbType.String, model.Name);

                if (model.GenderID == 0)
                    db.AddInParameter(cmd, "@GenderID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@GenderID", DbType.Int32, model.GenderID);

                if (model.BloodGroupID == 0)
                    db.AddInParameter(cmd, "@BloodGroupID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@BloodGroupID", DbType.Int32, model.BloodGroupID);

                if (string.IsNullOrEmpty(model.QualificationIDs))
                    db.AddInParameter(cmd, "@QualificationIDs", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@QualificationIDs", DbType.String, model.QualificationIDs);

                if (string.IsNullOrEmpty(model.SpecializationIDs))
                    db.AddInParameter(cmd, "@SpecializationIDs", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@SpecializationIDs", DbType.String, model.SpecializationIDs);


                if (model.MinFee == null || model.MinFee <= 0)
                    db.AddInParameter(cmd, "@MinFee", DbType.Decimal, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@MinFee", DbType.Decimal, model.MinFee);


                if (model.MaxFee == null || model.MaxFee <= 0)
                    db.AddInParameter(cmd, "@MaxFee", DbType.Decimal, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@MaxFee", DbType.Decimal, model.MaxFee);

                if (model.IsActive == null)
                    db.AddInParameter(cmd, "@IsActive", DbType.Boolean, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@IsActive", DbType.Boolean, model.IsActive);

                db.AddInParameter(cmd, "@PageNumber", DbType.Int32, model.PageNumber);
                db.AddInParameter(cmd, "@PageSize", DbType.Int32, model.PageSize);


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
                            QualificationID = Convert.ToInt32(row["QualificationID"]),
                            TalukaID = Convert.ToInt32(row["TalukaID"]),
                            QualificationName = row["QualificationName"].ToString(),
                            ConsultationFee = Convert.ToDecimal(row["ConsultationFee"]),
                            CRRIStartDate = row["CRRIStartDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["CRRIStartDate"]) : null,
                            DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["DateOfBirth"]) : null,
                            Address = row["Address"].ToString(),
                            TalukaName = row["TalukaName"].ToString(),
                            ProfilePicName = row["ProfilePicName"].ToString(),
                            ProfilePicPath = row["ProfilePicPath"].ToString(),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            SpecializationName = row["SpecializationName"].ToString(),
                            SpecializationIDs = row["SpecializationIDs"].ToString()
                        });

                    }
                    model.TotalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"]);
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, 1);
            }

            return doctorList;
        }
        #endregion


        #region Upsert(DoctorsModel model)
        public bool Upsert(DoctorsModel model)
        {
            string basePath = ConfigurationManager.AppSettings["ProfilePicturePath"];
            string fullFilePath = null;

            using (var scope = new TransactionScope())
            {
                try
                {
             
                    DbCommand com = db.GetStoredProcCommand("MedSync_DoctorUpsert");

                    db.AddInParameter(com, "DoctorID", DbType.Int32, model.DoctorID);


                    com.Parameters["@DoctorID"].Direction = ParameterDirection.InputOutput;

                    db.AddInParameter(com, "FirstName", DbType.String, model.FirstName);
                    db.AddInParameter(com, "LastName", DbType.String, model.LastName);
                    db.AddInParameter(com, "PhoneNumber", DbType.String, model.PhoneNumber);
                    db.AddInParameter(com, "GenderID", DbType.Int32, model.GenderID);
                    db.AddInParameter(com, "BloodGroupID", DbType.Int32, model.BloodGroupID);
                    db.AddInParameter(com, "QualificationID", DbType.Int32, model.QualificationID);
                    db.AddInParameter(com, "CRRIStartDate", DbType.Date, model.CRRIStartDate ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ConsultationFee", DbType.Decimal, model.ConsultationFee);
                    db.AddInParameter(com, "DateOfBirth", DbType.Date, model.DateOfBirth ?? (object)DBNull.Value);
                    db.AddInParameter(com, "Address", DbType.String, model.Address ?? (object)DBNull.Value);
                    db.AddInParameter(com, "TalukaID", DbType.Int32, model.TalukaID ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ProfilePicName", DbType.String, model.ProfilePicName ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ProfilePicPath", DbType.String, model.ProfilePicPath ?? (object)DBNull.Value);
                    db.AddInParameter(com, "CreatedBy", DbType.Int32, model.CreatedBy ?? (object)DBNull.Value);
                    db.AddInParameter(com, "ModifiedBy", DbType.Int32, model.ModifiedBy ?? (object)DBNull.Value);
                    db.AddInParameter(com, "SpecializationIDs", DbType.String, model.SpecializationIDs ?? (object)DBNull.Value);

                    db.ExecuteNonQuery(com);

                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    if (File.Exists(fullFilePath))
                        File.Delete(fullFilePath);
                    DbErrorLogger.LogError(ex, model.CreatedBy);
                    throw;
                }
            }
        }
        #endregion



    }
}



//if (model.ProfilePicFile != null && model.ProfilePicFile.ContentLength > 0)
//{
//    string originalFileName = Path.GetFileName(model.ProfilePicFile.FileName);
//    string guid = Guid.NewGuid().ToString();
//    string extension = Path.GetExtension(originalFileName);
//    string uniqueFileName = $"{guid}{extension}";
//    fullFilePath = Path.Combine(basePath, uniqueFileName);
//    Directory.CreateDirectory(basePath);
//    model.ProfilePicFile.SaveAs(fullFilePath);

//    model.ProfilePicName = originalFileName;
//    model.ProfilePicPath = uniqueFileName;
//}
