using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class Gender
    {
        private readonly Database db;

        public Gender()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region GetGendersList()
        public List<GenderModel> GetGendersList()
        {
            List<GenderModel> list = new List<GenderModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_GendersGetList");

                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                    {
                        list.Add(new GenderModel
                        {
                            GenderID = Convert.ToInt32(dr["GenderID"]),
                            GenderName = Convert.ToString(dr["GenderName"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
            }

            return list;
        }
        #endregion
    
    }
}
