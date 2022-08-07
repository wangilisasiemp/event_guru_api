using System;
namespace event_guru_api.models
{
    public class BudgetVendor : BaseEntity
    {
        public int BudgetID { get; set; }
        public Budget? Budget { get; set; }
        public int VendorID { get; set; }
        public Vendor? Vendor { get; set; }
    }
}

