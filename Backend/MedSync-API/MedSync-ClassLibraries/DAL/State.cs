using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MedSync_ClassLibraries.Models;
using MedSync_ClassLibraries.Helpers;

namespace MedSync_ClassLibraries.DAL
{
    public class State
    {
        private readonly Database db;
        public State() 
        { 
            db = DatabaseFactory.CreateDatabase(); 
        }

        #region GetStateList()
        public List<StateModel> GetStateList()
        {
            var list = new List<StateModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_StatesGetList");
                using (IDataReader dr = db.ExecuteReader(com))
                {
                    while (dr.Read())
                        list.Add(new StateModel
                        {
                            StateID = Convert.ToInt32(dr["StateID"]),
                            StateName = dr["StateName"].ToString()
                        });
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
