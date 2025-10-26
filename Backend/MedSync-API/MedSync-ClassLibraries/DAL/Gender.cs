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

        public List<GenderModel> GetAllGenders()
        {
            List<GenderModel> list = new List<GenderModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_GetAllGenders");

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
                Console.WriteLine("Error in GetAllGenders: " + ex.Message);
            }

            return list;
        }
    }
}
