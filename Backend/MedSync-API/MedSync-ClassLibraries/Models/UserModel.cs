using System;
using System.ComponentModel.DataAnnotations;

namespace MedSync_ClassLibraries.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public int? DoctorID { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^(?!.*\.\.)[A-Za-z0-9._%+-]+@[A-Za-z0-9-]+\.[A-Za-z]{2,}(?:\.[A-Za-z]{2,})?$", ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters and contain an uppercase letter, lowercase letter, number, and special character.")]
        public string PasswordHash { get; set; }



        public DateTime? LastLoginOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }



        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
