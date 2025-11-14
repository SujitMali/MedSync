using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MedSync_ClassLibraries.Models;
using MedSync_ClassLibraries.Helpers;

namespace MedSync_ClassLibraries.DAL
{
    public class District
    {
        private readonly Database db;

        public District()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region GetDistrictListByStateId(int? stateId = null)
        public List<DistrictModel> GetDistrictListByStateId(int? stateId = null)
        {
            var list = new List<DistrictModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_DistrictGetList");
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
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, 1);
            }
            return list;
        }
        #endregion
    
    }

}
