using System;
namespace event_guru_api.models
{
    public class EventAttendance : BaseEntity
    {

        public bool Confirmed { get; set; } = false;
        public bool Vegetarian { get; set; } = false;
        public bool Diabetic { get; set; } = false;
        public bool Alcoholic { get; set; } = false;
        public bool Halal { get; set; } = true;

        public string? AttendeeID { get; set; }
        public ApplicationUser? Attendee { get; set; }
        public int EventID { get; set; }
        public Event? Event { get; set; }
    }
}

