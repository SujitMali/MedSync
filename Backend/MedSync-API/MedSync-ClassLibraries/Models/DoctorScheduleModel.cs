using System;

namespace MedSync_ClassLibraries.Models
{
    public class DoctorScheduleModel
    {
        public int? DoctorScheduleID { get; set; }
        public int DoctorID { get; set; }
        public byte DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDurationID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
