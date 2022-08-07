using System;
using System.ComponentModel.DataAnnotations;

namespace event_guru_api.Controllers
{
    public class VendorModel
    {
        [Required(ErrorMessage = "The name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The Location is required")]
        public string? Location { get; set; }
        public string? Photo { get; set; }

        [Required(ErrorMessage = "The Phone is required")]
        public string? Phone { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "The Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Type of Vendor is required")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "The Unit of service is required")]
        public string? Unit { get; set; }

        [Required(ErrorMessage = "The Bronze Price is required")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "The Negotiability is required")]
        public bool? Negotiable { get; set; }
    }
}

