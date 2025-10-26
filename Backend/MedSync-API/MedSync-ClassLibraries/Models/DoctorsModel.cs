using System;
using System.Web;

namespace MedSync_ClassLibraries.Models
{
    public class DoctorsModel
    {
        //public int DoctorID { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        //public int GenderID { get; set; }
        //public int BloodGroupID { get; set; }
        //public int QualificationID { get; set; }
        //public DateTime? CRRIStartDate { get; set; }
        //public decimal ConsultationFee { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public string Address { get; set; }
        //public int? TalukaID { get; set; }
        //public bool IsActive { get; set; } = true;
        //public DateTime CreatedOn { get; set; } = DateTime.Now;
        //public int? CreatedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        //public int? ModifiedBy { get; set; }


        //public string ProfilePicName { get; set; }
        //public string ProfilePicPath { get; set; }
        //public HttpPostedFile ProfilePicFile { get; set; }

        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int GenderID { get; set; }
        public int BloodGroupID { get; set; }
        public int QualificationID { get; set; }
        public string QualificationName { get; set; }
        public DateTime? CRRIStartDate { get; set; }
        public decimal ConsultationFee { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public int? TalukaID { get; set; }
        public string TalukaName { get; set; }
        public bool IsActive { get; set; } = true;
        public string ProfilePicName { get; set; }
        public string ProfilePicPath { get; set; }
        public HttpPostedFile ProfilePicFile { get; set; }
        public string SpecializationName { get; set; }

        // Pagination & Sorting
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

        // Multi-select filters
        public string QualificationIDs { get; set; }
        public string SpecializationIDs { get; set; }
    }
}
