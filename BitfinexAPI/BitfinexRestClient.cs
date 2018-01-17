using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitfinexAPI.JsonConverters;
using BitfinexAPI.Resources;
using CommonRestClient;
using RestSharp;

namespace BitfinexAPI
{
    public class BitfinexRestClient : BaseRestClient
    {
        private const string BaseUrl = "https://api.bitfinex.com/";

        public BitfinexRestClient(string apiKey, string secretKey) : 
            base(apiKey, secretKey, BaseUrl)
        {
        }
        
        internal static T DeserializeObject<T>(string json, JsonConverter converter)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, converter);
            }
            catch (JsonReaderException ex)
            {
                throw new Exception("Error deserializing response", ex);
            }
        }

        protected override async Task<IRestResponse> GetResponseAsync(IRestRequest request)
        {
            var response = await base.GetResponseAsync(request);

            BitfinexException exception = null;
            try
            {
                exception = JsonConvert.DeserializeObject<BitfinexException>(
                    response.Content, new BitfinexExceptionConverter());
            }
            catch (Exception)
            {
                // ignored
            }

            if (exception != null)
            {
                throw new Exception($"({exception.ErrorCode}) {exception.Message}");
            }

            return response;
        }
        
        public async Task<PlatformStatus> GetPlatformStatusAsync()
        {
            var request = new RestRequest("v2/platform/status");

            var response = await GetResponseAsync(request);

            return DeserializeObject<PlatformStatus>(response.Content, new PlatformStatusConverter());
        }

        public async Task<List<Alert>> GetAlertsAsync()
        {
            var apiPath = "v2/auth/r/alerts";

            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            long nonce = (long)Math.Floor(diff.TotalMilliseconds);

            var rawBody = JsonConvert.SerializeObject(new
            {
                type = "price"
            });

            string signature = $"/api/{apiPath}{nonce}{rawBody}";
            var hmac = new HMACSHA384(Encoding.UTF8.GetBytes(SecretKey));
            byte[] k = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature));
            string signatureString = string.Concat(k.Select(b => b.ToString("X2")).ToArray()).ToLower();

            var request = new RestRequest(apiPath) { Method = Method.POST };
            request.AddHeader("bfx-nonce", nonce.ToString());
            request.AddHeader("bfx-apikey", ApiKey);
            request.AddHeader("bfx-signature", signatureString);
            request.AddHeader("Accept", "application/json");
            request.AddParameter("application/json; charset=utf-8", rawBody, ParameterType.RequestBody);

            var response = await GetResponseAsync(request);

            return DeserializeObject<List<Alert>>(response.Content, new AlertsConverter());
        }
    }   
}