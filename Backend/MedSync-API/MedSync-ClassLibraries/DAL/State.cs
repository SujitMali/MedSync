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
    public class State
    {
        private readonly Database db;
        public State() 
        { 
            db = DatabaseFactory.CreateDatabase(); 
        }

        public List<StateModel> GetAllStates()
        {
            var list = new List<StateModel>();
            try
            {
                DbCommand com = db.GetStoredProcCommand("MedSync_GetAllStates");
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
            catch (Exception ex) { Console.WriteLine("Error in GetAllStates: " + ex.Message); }
            return list;
        }
    }
}
