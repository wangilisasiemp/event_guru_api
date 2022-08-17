using System;
namespace event_guru_api.Services
{
    public interface ISMSSenderService
    {

        public Task<bool> SendSMS(SMSModel smsModel);
        public Task SendMultipleSMS(SMSMultipleModel sMSMultipleModel);
        public Task ReceiveSMS();
    }
}

