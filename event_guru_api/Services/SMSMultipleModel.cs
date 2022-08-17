using System;
namespace event_guru_api.Services
{
    public class SMSMultipleModel
    {
        public string? From { get; set; }
        public ICollection<String>? To { get; set; }
        public string? Text { get; set; }
    }
}

