using System;
namespace event_guru_api.models
{
    public class Vendor : BaseEntity
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Photo { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public string? Unit { get; set; }
        public double? Price { get; set; }
        public bool? Negotiable { get; set; }

        public ICollection<BudgetVendor>? BudgetVendors { get; set; }
    }
}

