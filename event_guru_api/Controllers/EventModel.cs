using System;
using System.ComponentModel.DataAnnotations;

namespace event_guru_api.Controllers
{
    public class EventModel
    {
        [Required(ErrorMessage = "The title of the event is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The Description of the event is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The Location of the event is required")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "The Contacts of the event is required")]
        public string? Contact { get; set; }


        [Required(ErrorMessage = "The event type is required")]
        public string? Type { get; set; }

        public double? MinContribution { get; set; } = 0;
        public string? SecretCode { get; set; }

        [Required(ErrorMessage = "The date of the event is required")]
        public DateTime EventDate { get; set; }

        public DateTime? FinalizationDate { get; set; }

        public bool? Caterer { get; set; } = false;
        public bool? Drinks { get; set; } = true;
        public bool? MC { get; set; } = false;
        public bool? ConferenceHall { get; set; } = true;
        public bool? RoyalTransport { get; set; } = false;
        public bool? Decoration { get; set; } = true;
        public bool? OrdinaryTransport { get; set; } = false;
        public bool? Security { get; set; } = false;
        public bool? Entertainment { get; set; } = false;


        //Will be entered in ranges of 100 i.e. (0-100,101-200,201-300,301-400,401-500,501-1000)
        [Required(ErrorMessage = "The number of attendees is required")]
        public int? NoOfAttendees { get; set; }


        [Required(ErrorMessage = "The Starting time of the event is required")]
        public TimeOnly? EventStartTime { get; set; }

        [Required(ErrorMessage = "The Ending time of the event is required")]
        public TimeOnly? EventEndTime { get; set; }

        [Required(ErrorMessage = "The Organizer of the event is required")]
        public string? OrganizerID { get; set; }
    }
}

