using System;
using System.ComponentModel.DataAnnotations;

namespace event_guru_api.Controllers
{
    public class ContributionModel
    {
        [Required(ErrorMessage = "The amount of contribution is required")]
        public double? Amount { get; set; }

        [Required(ErrorMessage = "The Contribution status is required")]
        public bool? Completed { get; set; }

        [Required(ErrorMessage = "The id of the event for the contribution is required")]
        public int? EventID { get; set; }

        [Required(ErrorMessage = "The attendee who contributed is required")]
        public string? AttendeeID { get; set; }

    }
}

