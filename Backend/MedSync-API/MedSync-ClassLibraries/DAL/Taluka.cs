using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class Taluka
    {
        private readonly Database db;
        public Taluka() { db = DatabaseFactory.CreateDatabase(); }

        public List<TalukaModel> GetTalukaByDistrict(int? districtId = null)
        {
            var list = new List<TalukaModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_GetAllTaluka");
                db.AddInParameter(com, "@DistrictID", DbType.Int32, districtId);

                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                        list.Add(new TalukaModel
                        {
                            TalukaID = Convert.ToInt32(dr["TalukaID"]),
                            TalukaName = dr["TalukaName"].ToString(),
                            DistrictID = Convert.ToInt32(dr["DistrictID"])
                        });
                }
            }
            catch (Exception ex) { Console.WriteLine("Error in GetTalukaByDistrict: " + ex.Message); }
            return list;
        }
    }

}
