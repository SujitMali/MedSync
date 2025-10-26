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

        public List<BloodGroupModel> GetAllBloodGroups()
        {
            List<BloodGroupModel> list = new List<BloodGroupModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_GetAllBloodGroups");

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
                Console.WriteLine("Error in GetAllBloodGroups: " + ex.Message);
            }

            return list;
        }
    }
}
