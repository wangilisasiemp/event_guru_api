using System;
namespace event_guru_api.Controllers
{
    public class PaymentRequestModel
    {
        public double input_Amount { get; set; }
        public string input_Country { get; set; } = "TZN";
        public string input_Currency { get; set; } = "TZS";
        public string input_CustomerMSISDN { get; set; } = "000000000001";
        public string input_ThirdPartyConversationID { get; set; }
        public string input_TransactionReference { get; set; }
        public string input_PurchasedItemsDesc { get; set; } = "Event Attendance";
    }
}

