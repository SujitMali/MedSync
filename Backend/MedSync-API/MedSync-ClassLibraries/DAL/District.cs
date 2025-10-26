using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedSync_ClassLibraries.Models;

namespace MedSync_ClassLibraries.DAL
{
    public class District
    {
        private readonly Database db;
        public District() { db = DatabaseFactory.CreateDatabase(); }

        public List<DistrictModel> GetDistrictsByState(int? stateId = null)
        {
            var list = new List<DistrictModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_GetAllDistrict");
                db.AddInParameter(com, "@StateID", DbType.Int32, stateId);
                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                        list.Add(new DistrictModel
                        {
                            DistrictID = Convert.ToInt32(dr["DistrictID"]),
                            DistrictName = dr["DistrictName"].ToString(),
                            StateID = Convert.ToInt32(dr["StateID"])
                        });
                }
            }
            catch (Exception ex) { Console.WriteLine("Error in GetDistrictsByState: " + ex.Message); }
            return list;
        }
    }

}
