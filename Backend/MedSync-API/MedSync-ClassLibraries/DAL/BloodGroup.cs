using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class BloodGroup
    {
        private readonly Database db;

        public BloodGroup()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        #region  GetBloodGroupList()
        public List<BloodGroupModel> GetBloodGroupList()
        {
            List<BloodGroupModel> list = new List<BloodGroupModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_BloodGroupsGetList");

                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                    {
                        list.Add(new BloodGroupModel
                        {
                            BloodGroupID = Convert.ToInt32(dr["BloodGroupID"]),
                            BloodGroupName = Convert.ToString(dr["BloodGroupName"])
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

