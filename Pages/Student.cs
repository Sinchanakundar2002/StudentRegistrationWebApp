using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApp1.Models
{
    public class Student
    {
        public string Id { get; set; } = string.Empty;
        public string RegistrationID => $"STD{Id}";


        [Required(ErrorMessage = "School Name is required")]
        public string SchoolName { get; set; }


        [Required(ErrorMessage = "Class Name is required")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters")]
        public string StudentName { get; set; }


        [Required(ErrorMessage = "DOB is required")]
        [CustomValidation(typeof(Student), nameof(ValidateAge))]
        public DateTime DateOfBirth { get; set; }
        public static ValidationResult? ValidateAge(DateTime dob, ValidationContext context)
        {
            int age = DateTime.Today.Year - dob.Year;
            if (dob > DateTime.Today.AddYears(-age)) age--;

            if (age < 12 || age > 17)
            {
                return new ValidationResult("Student must be between 12 and 17 years old.");
            }

            return ValidationResult.Success;
        }


        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be exactly 10 digits")]
        public string ContactNumber { get; set; }


        [Required(ErrorMessage = "Address is required")]
        public Address Address { get; set; } = new Address();

        public string Status { get; set; } = "Active"; // Possible values: "Active", "Deleted"
    }
    public class Address
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }

}

