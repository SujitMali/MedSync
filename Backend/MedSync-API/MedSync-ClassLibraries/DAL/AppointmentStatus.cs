using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MedSync_ClassLibraries.Helpers;

namespace MedSync_ClassLibraries.DAL
{
    public class AppointmentStatus
    {
        private readonly Database db;

        public AppointmentStatus()
        {
            db = DatabaseFactory.CreateDatabase();
        }


        #region GetAppointmentStatusList()
        public List<AppointmentStatusModel> GetAppointmentStatusList(int CurrentUserId)
        {
            var list = new List<AppointmentStatusModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_AppointmentStatusesGetList");
                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                        list.Add(new AppointmentStatusModel
                        {
                            AppointmentStatusID = Convert.ToInt32(dr["AppointmentStatusID"]),
                            StatusName = dr["StatusName"].ToString()
                        });
                }
            }
            catch (Exception ex) 
            { 
                DbErrorLogger.LogError(ex, CurrentUserId); 
            }
            return list;
        }

        #endregion


    }
}
