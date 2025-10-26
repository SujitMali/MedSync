using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSync_ClassLibraries.DAL
{
    public class SlotDuration
    {
        private readonly Database db;

        public SlotDuration()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public List<SlotDurationModel> GetAllSlotDurations()
        {
            var list = new List<SlotDurationModel>();
            DbCommand cmd = db.GetStoredProcCommand("MedSync_GetSlotDurations");

            using (var reader = db.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    list.Add(new SlotDurationModel
                    {
                        SlotDurationID = Convert.ToInt32(reader["SlotDurationID"]),
                        DurationMinutes = Convert.ToInt32(reader["DurationMinutes"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            return list;
        }
    }
}
