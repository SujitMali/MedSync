using System;


namespace MedSync_ClassLibraries.Models
{
    public class AppointmentStatusModel
    {
        public int AppointmentStatusID { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
