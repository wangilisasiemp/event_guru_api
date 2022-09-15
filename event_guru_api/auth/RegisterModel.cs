using System;
using System.ComponentModel.DataAnnotations;

namespace event_guru_api.auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        public string? Address { get; set; }


        [Required(ErrorMessage = "Password is required")]
        //[RegularExpression(@"^(?=.*[a - z])(?=.*[A - Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$",
        [RegularExpression(@"(?=.*?[A-Z])(?=(.*[a-z]))(?=(.*[\d]))(?=(.*[^a-zA-Z0-9])).{6,}$")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        public string? ConfirmPassword { get; set; }
    }
}

