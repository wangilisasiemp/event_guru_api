using System;
using event_guru_api.Controllers;

namespace event_guru_api.Services
{
    public interface IMPesaService
    {
        public Task<PaymentSessionResponse> getSession();

        public Task<PaymentResponse> pay(PaymentRequestModel model);
    }
}

