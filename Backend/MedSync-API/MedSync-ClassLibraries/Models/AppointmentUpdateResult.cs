using System;

namespace MedSync_ClassLibraries.Models
{
    public class AppointmentUpdateResult
    {
        public int SuccessFlag { get; set; }
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string AppointmentStatus { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string CancellationReason { get; set; }
        public int DoctorID { get; set; }
    }

}
