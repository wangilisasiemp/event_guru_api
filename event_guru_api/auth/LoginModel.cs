using System;
using System.ComponentModel.DataAnnotations;

namespace event_guru_api.Controllers
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

