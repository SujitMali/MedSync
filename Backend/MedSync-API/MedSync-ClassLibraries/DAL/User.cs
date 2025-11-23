using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class User
    {
        private readonly Database db;

        public User()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        #region Insert(UserModel model)
        public bool Insert(UserModel model)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_UsersInsert");

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);


                if (model.DoctorID == 0)
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@DoctorID", DbType.Int32, model.DoctorID);

   
                if (string.IsNullOrWhiteSpace(model.Email))
                    db.AddInParameter(cmd, "@Email", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@Email", DbType.String, model.Email.Trim());


                if (string.IsNullOrWhiteSpace(model.PasswordHash))
                    db.AddInParameter(cmd, "@Password", DbType.String, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@Password", DbType.String, hashedPassword);

     
                if (model.RoleID == 0)
                    db.AddInParameter(cmd, "@RoleID", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@RoleID", DbType.Int32, model.RoleID);


                if (model.CreatedBy == 0 || model.CreatedBy == null)
                    db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, 1); 
                else
                    db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, model.CreatedBy);

   
                int rowsAffected = db.ExecuteNonQuery(cmd);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, model.CreatedBy);
                return false;
            }
        }
        #endregion


        #region GetUserByEmail(UserModel userInput)
        public UserModel GetUserByEmail(UserModel userInput)
        {
            UserModel user = null;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_UsersGetByEmail");
                db.AddInParameter(cmd, "@Email", DbType.String, userInput.Email);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    if (dr.Read())
                    {
                        string storedHash = Convert.ToString(dr["PasswordHash"]);

                        if (!BCrypt.Net.BCrypt.Verify(userInput.PasswordHash, storedHash))
                            return null;

                        user = new UserModel
                        {
                            UserID = Convert.ToInt32(dr["UserID"]),
                            RoleID = Convert.ToInt32(dr["RoleID"]),
                            DoctorID = dr["DoctorID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["DoctorID"]),
                            Email = Convert.ToString(dr["Email"]),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                            RoleName = Convert.ToString(dr["RoleName"]),
                            FirstName = dr["FirstName"] == DBNull.Value ? null : Convert.ToString(dr["FirstName"]),
                            LastName = dr["LastName"] == DBNull.Value ? null : Convert.ToString(dr["LastName"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, user.UserID);
            }

            return user;
        }
        #endregion


        #region GetDoctorsWithoutCredentials
        public List<DoctorsModel> GetDoctorsWithoutCredentials()
        {
            var doctors = new List<DoctorsModel>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_UsersGetDoctorsNotInUsers");

                using (var reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var doctor = new DoctorsModel
                        {
                            DoctorID = Convert.ToInt32(reader["DoctorID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            //Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            QualificationID = reader["QualificationID"] != DBNull.Value
                                ? Convert.ToInt32(reader["QualificationID"])
                                : 0,
                            QualificationName = reader["QualificationName"] != DBNull.Value
                                ? reader["QualificationName"].ToString()
                                : string.Empty,
                            IsActive = reader["IsActive"] != DBNull.Value
                                ? Convert.ToBoolean(reader["IsActive"])
                                : false,
                            CreatedOn = reader["CreatedOn"] != DBNull.Value
                                ? Convert.ToDateTime(reader["CreatedOn"])
                                : DateTime.MinValue
                        };

                        doctors.Add(doctor);
                    }
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, 1);
            }

            return doctors;
        }
        #endregion


    }
}
