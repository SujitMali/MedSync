using System;

namespace MedSync_ClassLibraries.Models
{
    public class AppointmentFileModel
    {
        public int AppointmentFileID { get; set; }
        public int AppointmentID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int? UploadedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }

}
