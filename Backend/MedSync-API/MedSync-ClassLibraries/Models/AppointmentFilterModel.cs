using System;

namespace MedSync_ClassLibraries.Models
{
    public class AppointmentFilterModel
    {
        public int? DoctorID { get; set; }
        public string StatusIDs { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string PatientName { get; set; }
        public bool? IsDoctorSuggestedChange { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string SortColumn { get; set; } = "AppointmentDate";
        public string SortDirection { get; set; } = "ASC";
        public string SpecializationIDs { get; set; } 
    }
}
