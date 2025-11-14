using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class Qualification
    {
        private readonly Database db;

        public Qualification()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region GetQualificationsList()
        public List<QualificationModel> GetQualificationsList()
        {
            List<QualificationModel> list = new List<QualificationModel>();

            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_QualificationsGetList");

                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                    {
                        list.Add(new QualificationModel
                        {
                            QualificationID = Convert.ToInt32(dr["QualificationID"]),
                            QualificationName = Convert.ToString(dr["QualificationName"])
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
    