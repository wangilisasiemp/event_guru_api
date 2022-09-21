using System;
namespace event_guru_api.Controllers
{
    public class ContributionResultModel
    {
        public int? ID { get; set; }
        public double? Amount { get; set; }
        public Boolean? Completed { get; set; }
        public int? EventID { get; set; }
        public string? TransactionID { get; set; }
        public string? EventName { get; set; }
        public String? Contributor { get; set; }

    }
}

