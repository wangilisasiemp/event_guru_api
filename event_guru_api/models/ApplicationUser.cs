using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace event_guru_api.models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }

        public ICollection<EventAttendance>? EventAttendances { get; set; }
        public ICollection<Event>? Events { get; set; }
        public ICollection<Contribution>? Contributions { get; set; }
        public ICollection<Invitation>? Invitations { get; set; }
    }
}

