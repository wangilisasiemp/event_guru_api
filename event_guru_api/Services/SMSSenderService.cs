using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using NuGet.Protocol;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace event_guru_api.Services
{
    public class SMSSenderService : ISMSSenderService
    {
        private readonly string single_SMS_url;
        private readonly string multiple_SMS_url;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private ILogger<SMSSenderService> _logger;
        public SMSSenderService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<SMSSenderService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            single_SMS_url = _configuration["SINGLE_DESTINATION_URL"];
            multiple_SMS_url = _configuration["MULTIPLE_DESTINATION_URL"];
            _logger = logger;
        }

        public Task ReceiveSMS()
        {
            throw new NotImplementedException();
        }

        public Task SendMultipleSMS(SMSMultipleModel sMSMultipleModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendSMS(SMSModel smsModel)
        {
            var smsJson = new StringContent(JsonSerializer.Serialize(smsModel), Encoding.UTF8, Application.Json);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://messaging-service.co.tz/api/sms/v1/text/single")
            {
                Headers =
                {
                    {HeaderNames.Accept,"application/json" },
                    {HeaderNames.UserAgent,"HttpRequestSample" },
                    {HeaderNames.Authorization,"Basic d2FuZ2lsaXNhc2llbXA6Km1ueWlAZHVuZGFANzg5MSM=" }
                },
            };
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "d2FuZ2lsaXNhc2llbXA6Km1ueWlAZHVuZGFANzg5MSM =");
            var httpResponseMessage = await httpClient.PostAsync("https://messaging-service.co.tz/api/sms/v1/text/single", smsJson);
            //var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogInformation(await httpResponseMessage.Content.ReadAsStringAsync());
                return true;
            }

            return false;
        }
    }
}

