using System;
namespace event_guru_api.Services
{
    public interface IEmailSenderService
    {
        public Task SendEmail(string emailModel);
        public Task ReceiveEmail();
    }
}

