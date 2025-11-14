using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MedSync_ClassLibraries.Helpers;

namespace MedSync_ClassLibraries.DAL
{
    public class Specialization
    {
        private readonly Database db;

        public Specialization()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region GetSpecializationList()
        public List<SpecializationModel> GetSpecializationList()
        {
            List<SpecializationModel> list = new List<SpecializationModel>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_SpecializationsGetList");

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    while (dr.Read())
                    {
                        list.Add(new SpecializationModel
                        {
                            SpecializationID = Convert.ToInt32(dr["SpecializationID"]),
                            SpecializationName = Convert.ToString(dr["SpecializationName"])
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
