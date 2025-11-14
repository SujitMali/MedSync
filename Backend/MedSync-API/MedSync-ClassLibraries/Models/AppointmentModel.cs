using System;
using System.ComponentModel.DataAnnotations;

namespace MedSync_ClassLibraries.Models
{
    public class AppointmentModel
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AppointmentStatusID { get; set; }
        public string StatusName { get; set; }
        public string CancellationReason { get; set; }
        public bool IsDoctorSuggestedChange { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }

        public string SpecializationNames { get; set; }


        public string MedicalHistory { get; set; }
        public string MedicalConcern { get; set; }
        public string InsuranceDetails { get; set; }

        public string PatientGender { get; set; }
        public string PatientBloodGroup { get; set; }
        public DateTime? PatientDOB { get; set; }

    }
}
