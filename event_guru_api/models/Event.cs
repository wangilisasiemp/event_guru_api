using System;
namespace event_guru_api.models
{
    public class Event : BaseEntity
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Contact { get; set; }
        public double? MinContribution { get; set; } = 0;
        public string? SecretCode { get; set; }
        public string? Type { get; set; }
        public int? NoOfAttendees { get; set; }

        public double? EventBudget { get; set; }
        public bool Caterer { get; set; } = false;
        public bool Drinks { get; set; } = true;
        public bool MC { get; set; } = false;
        public bool ConferenceHall { get; set; } = true;
        public bool RoyalTransport { get; set; } = false;
        public bool Decoration { get; set; } = true;
        public bool OrdinaryTransport { get; set; } = false;
        public bool Security { get; set; } = false;
        public bool Entertainment { get; set; } = false;


        public DateTime? EventDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public TimeOnly? EventStartTime { get; set; }
        public TimeOnly? EventEndTime { get; set; }


        public string? OrganizerID { get; set; }
        public ApplicationUser? Organizer { get; set; }

        public ICollection<EventAttendance>? EventAttendances { get; set; }
        public ICollection<Contribution>? Contributions { get; set; }
        public ICollection<Invitation>? Invitations { get; set; }
        public ICollection<Budget>? Budgets { get; set; }
    }
}

