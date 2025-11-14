using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace MedSync_ClassLibraries.DAL
{
    public class SlotDuration
    {
        private readonly Database db;

        public SlotDuration()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region GetSlotDurationList()
        public List<SlotDurationModel> GetSlotDurationList(int CurrentUserId)
        {
            var list = new List<SlotDurationModel>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_SlotDurationsGetList");

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
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: CurrentUserId);
            }

            return list;
        }

        #endregion

    }
}

