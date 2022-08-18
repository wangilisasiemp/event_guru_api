using System;
namespace event_guru_api.models
{
    public class Invitation : BaseEntity
    {
        public int ID { get; set; }
        public string? CardText { get; set; }
        public string? EventLink { get; set; }

        public int? EventID { get; set; }
        public Event? Event { get; set; }

        public string? AttendeeID { get; set; }
        public ApplicationUser? Attendee { get; set; }
    }
}

