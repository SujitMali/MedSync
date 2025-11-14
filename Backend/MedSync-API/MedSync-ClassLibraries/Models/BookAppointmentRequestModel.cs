using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MedSync_ClassLibraries.Models
{
    public class BookAppointmentRequestModel
    {
        [Required(ErrorMessage = "DoctorID is required.")]
        public int DoctorID { get; set; }



        [Required(ErrorMessage = "Appointment Date is required.")]
        public DateTime AppointmentDate { get; set; }


        [Required(ErrorMessage = "Start Time is required.")]
        public TimeSpan StartTime { get; set; }


        [Required(ErrorMessage = "End Time is required.")]
        public TimeSpan EndTime { get; set; }


        public int CreatedBy { get; set; }



        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
        public string Patient_FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
        public string Patient_LastName { get; set; }



        [Required(ErrorMessage = "Gender is required.")]
        public int Patient_GenderID { get; set; }



        public int? Patient_BloodGroupID { get; set; }


        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime? Patient_DateOfBirth { get; set; }



        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Patient_PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Patient_Email { get; set; }



        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string Patient_Address { get; set; }



        [Required(ErrorMessage = "Taluka selection is required.")]
        public int? Patient_TalukaID { get; set; }





        [StringLength(300, ErrorMessage = "Medical history cannot exceed 300 characters.")]
        public string MedicalHistory { get; set; }



        [Required(ErrorMessage = "Medical Concern is mandatory.")]
        [StringLength(300, ErrorMessage = "Medical concern cannot exceed 300 characters.")]
        public string MedicalConcern { get; set; }



        [StringLength(200, ErrorMessage = "Insurance details cannot exceed 200 characters.")]
        public string InsuranceDetails { get; set; }



        public List<AppointmentFileModel> Files { get; set; } = new List<AppointmentFileModel>();
        public List<HttpPostedFile> PostedFiles { get; set; }
    }
}
