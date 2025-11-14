using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MedSync_ClassLibraries.Models
{
    public class DoctorsModel
    {
        public int DoctorID { get; set; }



        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression(@"^[A-Za-z][A-Za-z\s]*$", ErrorMessage = "First Name can only contain letters and spaces, starting with a letter.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }




        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression(@"^[A-Za-z][A-Za-z\s]*$", ErrorMessage = "Last Name can only contain letters and spaces, starting with a letter.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        public string LastName { get; set; }



        public string Email { get; set; }
        


        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "Gender is required.")]
        public int GenderID { get; set; }




        [Required(ErrorMessage = "Blood Group is required.")]
        public int BloodGroupID { get; set; }



        [Required(ErrorMessage = "Qualification is required.")]
        public int QualificationID { get; set; }




        [Required(ErrorMessage = "Consultation Fee is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Consultation Fee must be greater than 0.")]
        public decimal ConsultationFee { get; set; }




        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime? DateOfBirth { get; set; }




        [Required(ErrorMessage = "CRRI Start Date is required.")]
        public DateTime? CRRIStartDate { get; set; }




        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 250 characters.")]
        public string Address { get; set; }




        [Required(ErrorMessage = "Taluka is required.")]
        public int? TalukaID { get; set; }




        [Required(ErrorMessage = "At least one specialization must be selected.")]
        public string SpecializationIDs { get; set; }




        public string QualificationName { get; set; }
        public string TalukaName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string ProfilePicName { get; set; }
        public string ProfilePicPath { get; set; }
        public HttpPostedFile ProfilePicFile { get; set; }
        public string SpecializationName { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string QualificationIDs { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? MaxFee { get; set; }
        public string Name { get; set; }
    }
}
