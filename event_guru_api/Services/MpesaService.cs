using System;
using System.Text;
using event_guru_api.Controllers;
using PortalSDK;

namespace event_guru_api.Services
{
    public class MpesaService : IMPesaService
    {
        private readonly IConfiguration Config;
        public MpesaService(IConfiguration config)
        {
            this.Config = config;
        }


        public async Task<PaymentSessionResponse> getSession()
        {
            PaymentSessionResponse payResponse;
            //Api Key
            var api_key = Config["MPESA_SANDBOX:API_KEY"];

            //Public key on the API listener used to encrypt keys
            var public_key = Config["MPESA_SANDBOX:PUBLIC_KEY"];
            var address = Config["MPESA_SANDBOX:GET_SESSION_ADDRESS"];
            var port = Config["MPESA_SANDBOX:GET_SESSION_PORT"];
            var path = Config["MPESA_SANDBOX:GET_SESSION_PATH"];
            APIContext context = new APIContext();
            context.setPublicKey(public_key);
            context.setApiKey(api_key);
            context.setSsl(true);
            context.setMethodType(APIMethodTypes.GET);
            context.setAddress(address);
            context.setPort(443);

            context.setPath(path);

            //context.addParameter("key", "value");
            context.addHeader("Origin", "*");

            APIRequest request = new APIRequest(context);
            APIResponse response = null;

            response = request.excecute();
            var body = response.getBody();
            payResponse = new PaymentSessionResponse()
            {
                Output_ResponseCode = body.GetValueOrDefault("output_ResponseCode"),
                Output_ResponseDesc = body.GetValueOrDefault("output_ResponseDesc"),
                Output_SessionID = body.GetValueOrDefault("output_SessionID")
            };
            return payResponse;
        }

        public async Task<PaymentResponse> pay(PaymentRequestModel model)
        {
            PaymentResponse payResponse;

            var paymentSessionResponse = await this.getSession();

            var sessionId = paymentSessionResponse.Output_SessionID;
            //Api Key
            var api_key = Config["MPESA_SANDBOX:API_KEY"];

            //Public key on the API listener used to encrypt keys
            var public_key = Config["MPESA_SANDBOX:PUBLIC_KEY"];
            var address = Config["MPESA_SANDBOX:POST_C2B_ADDRESS"];
            var port = Config["MPESA_SANDBOX:GET_SESSION_PORT"];
            var path = Config["MPESA_SANDBOX:POST_C2B_PATH"];
            APIContext context = new APIContext();
            context.setPublicKey(public_key);
            context.setApiKey(sessionId);
            context.setSsl(true);
            context.setMethodType(APIMethodTypes.POST);
            context.setAddress(address);
            context.setPort(443);

            context.setPath(path);

            //context.addParameter("key", "value");
            Random rnd = new Random();
            var transRef = generateTransRef();
            var nonHyphenatedGuid = model.input_ThirdPartyConversationID.Split("-");
            var convId = string.Join("", nonHyphenatedGuid);

            context.addParameter("input_Amount", model.input_Amount.ToString());
            context.addParameter("input_Country", "TZN");
            context.addParameter("input_Currency", "TZS");
            context.addParameter("input_CustomerMSISDN", "000000000001");
            context.addParameter("input_ServiceProviderCode", "000000");
            context.addParameter("input_ThirdPartyConversationID", convId);
            context.addParameter("input_TransactionReference", transRef);
            context.addParameter("input_PurchasedItemsDesc", "Event Attendance");
            context.addHeader("Origin", "*");

            APIRequest request = new APIRequest(context);
            Thread.Sleep(30 * 1000);
            APIResponse response = null;

            response = request.excecute();
            var body = response.getBody();
            payResponse = new PaymentResponse()
            {
                output_ConversationID = body.GetValueOrDefault("output_ConversationID"),
                output_ResponseCode = body.GetValueOrDefault("output_ResponseCode"),
                output_ResponseDesc = body.GetValueOrDefault("output_ResponseDesc"),
                output_TransactionID = body.GetValueOrDefault("output_TransactionID"),
                output_ThirdPartyConversationID = body.GetValueOrDefault("output_ThirdPartyConversationID")
            };
            return payResponse;
        }

        private string generateTransRef()
        {
            const string src = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int length = 20;
            var sb = new StringBuilder();
            Random rnd = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[rnd.Next(0, src.Length)];
                sb.Append(c);
            }
            return sb.ToString();
        }
    }
}

