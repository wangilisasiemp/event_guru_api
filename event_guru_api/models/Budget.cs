using System;
namespace event_guru_api.models
{
    public class Budget
    {
        public int ID { get; set; }
        public string? BudgetType { get; set; }
        public double? Value { get; set; }
        public int EventID { get; set; }
        public Event? Event { get; set; }

        public ICollection<BudgetVendor>? BudgetVendors { get; set; }
    }
}

