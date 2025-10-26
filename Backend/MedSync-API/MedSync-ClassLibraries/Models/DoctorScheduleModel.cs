using System;

namespace MedSync_ClassLibraries.Models
{
    public class DoctorScheduleModel
    {
        //public int DoctorScheduleID { get; set; }
        //public int DoctorID { get; set; }
        //public byte DayOfWeek { get; set; }
        ////public TimeSpan StartTime { get; set; }
        ////public TimeSpan EndTime { get; set; }   

        //public DateTime StartTime { get; set; }
        //public DateTime EndTime { get; set; }
        //public int SlotDurationID { get; set; }
        //public bool IsActive { get; set; } = true;
        //public DateTime CreatedOn { get; set; } = DateTime.Now;
        //public int? CreatedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        //public int? ModifiedBy { get; set; }

        public int? DoctorScheduleID { get; set; }   // nullable: new rows may not have an ID
        public int DoctorID { get; set; }
        public byte DayOfWeek { get; set; }          // 1=Sunday .. 7=Saturday (DB)
        public TimeSpan StartTime { get; set; }      // TIME
        public TimeSpan EndTime { get; set; }        // TIME
        public int SlotDurationID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
