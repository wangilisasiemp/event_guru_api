using System;
namespace event_guru_api.models
{
    public class Contribution : BaseEntity
    {
        public int? ID { get; set; }
        public double? Amount { get; set; }
        public bool? Completed { get; set; }

        public string? TransactionID { get; set; }
        public string? CustomerMSISDN { get; set; }
        public string? ConversationID { get; set; }
        public string? ResponseCode { get; set; }
        public string? ResponseDesc { get; set; }
        public string? ThirdPartyConversationID { get; set; }

        public int? EventID { get; set; }
        public Event? Event { get; set; }

        public string? AttendeeID { get; set; }
        public ApplicationUser? Attendee { get; set; }

    }
}

