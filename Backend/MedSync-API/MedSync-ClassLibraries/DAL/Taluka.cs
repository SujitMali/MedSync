using MedSync_ClassLibraries.Helpers;
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

        public Taluka()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region GetTalukaListByDistrictId(int? districtId = null)
        public List<TalukaModel> GetTalukaListByDistrictId(int? districtId = null)
        {
            var list = new List<TalukaModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_TalukasGetList");
                db.AddInParameter(com, "@DistrictID", DbType.Int32, districtId);

                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                    {
                        list.Add(new TalukaModel
                        {
                            TalukaID = Convert.ToInt32(dr["TalukaID"]),
                            TalukaName = dr["TalukaName"].ToString(),
                            DistrictID = Convert.ToInt32(dr["DistrictID"])
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

