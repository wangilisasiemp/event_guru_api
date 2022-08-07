using System;
using System.ComponentModel.DataAnnotations;

namespace event_guru_api.Controllers
{
    public class InvitationModel
    {
        [Required(ErrorMessage = "Event ID is required")]
        public int? EventID { get; set; }

        [Required(ErrorMessage = "Attendee ID is required")]
        public string? AttendeeID { get; set; }
    }
}

