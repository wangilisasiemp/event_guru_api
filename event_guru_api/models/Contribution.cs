using System;
namespace event_guru_api.models
{
    public class Contribution : BaseEntity
    {
        public int? ID { get; set; }
        public double? Amount { get; set; }
        public bool? Completed { get; set; }

        public int? EventID { get; set; }
        public Event? Event { get; set; }

        public string? AttendeeID { get; set; }
        public ApplicationUser? Attendee { get; set; }
    }
}

