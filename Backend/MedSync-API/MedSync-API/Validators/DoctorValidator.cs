using MedSync_ClassLibraries.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedSync_API.Validators
{
    public static class DoctorValidator
    {
        public static List<string> ValidateDoctorModel(DoctorsModel doctor)
        {
            var errors = new List<string>();
            var context = new ValidationContext(doctor, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(doctor, context, results, true);
            errors.AddRange(results.Select(r => r.ErrorMessage));

            // Custom date validations
            errors.AddRange(ValidateDoctorDates(doctor));

            return errors;
        }

        private static List<string> ValidateDoctorDates(DoctorsModel doctor)
        {
            var errors = new List<string>();

            if (!doctor.DateOfBirth.HasValue)
            {
                errors.Add("Date of Birth is required.");
                return errors;
            }

            var today = DateTime.Today;

            // ----------- DOB VALIDATION -----------
            if (doctor.DateOfBirth.Value >= today)
            {
                errors.Add("Date of Birth cannot be in the future.");
            }
            else
            {
                var dob = doctor.DateOfBirth.Value;
                var age = today.Year - dob.Year;

                if (dob.Date > today.AddYears(-age))
                    age--;

                const int MIN_AGE = 23;
                const int MAX_AGE = 90;

                if (age < MIN_AGE)
                    errors.Add($"Doctor must be at least {MIN_AGE} years old.");

                if (age > MAX_AGE)
                    errors.Add($"Doctor age cannot exceed {MAX_AGE} years.");
            }

            // ----------- CRRI DATE VALIDATION -----------
            if (!doctor.CRRIStartDate.HasValue)
            {
                errors.Add("CRRI Start Date is required.");
                return errors;
            }

            if (doctor.CRRIStartDate.Value >= today)
                errors.Add("CRRI Start Date must be before today.");

            // CRRI must be at least 23 years after DOB
            if (doctor.DateOfBirth.HasValue)
            {
                var minValidCRRI = doctor.DateOfBirth.Value.AddYears(23);
                if (doctor.CRRIStartDate.Value < minValidCRRI)
                    errors.Add("CRRI Start Date must be at least 23 years after Date of Birth.");
            }

            return errors;
        }
    
    }
}
