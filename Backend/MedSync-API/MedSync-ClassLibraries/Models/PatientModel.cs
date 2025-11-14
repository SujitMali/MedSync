using System;


namespace MedSync_ClassLibraries.Models
{
    public class PatientModel
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderID { get; set; }
        public int? BloodGroupID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int? TalukaID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

    }

}
