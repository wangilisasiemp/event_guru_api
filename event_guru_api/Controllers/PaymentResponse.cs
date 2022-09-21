using System;
namespace event_guru_api.Controllers
{
    public class PaymentResponse
    {
        public string output_ConversationID { get; set; }
        public string output_ResponseCode { get; set; }
        public string output_ResponseDesc { get; set; }
        public string output_TransactionID { get; set; }
        public string output_ThirdPartyConversationID { get; set; }
    }
}

