using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitfinexApi.JsonConverters;
using BitfinexApi.Resources;
using RestSharp;
using BaseApiClient;

namespace BitfinexApi
{
    public partial class BitfinexApiClient : BaseRestClient
    {
        private const string BaseUrl = "https://api.bitfinex.com/";

        public BitfinexApiClient(string apiKey, string secretKey) : 
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
        
        public async Task<T> GetResourceAsync<T>(IRestRequest restRequest, JsonConverter jsonConverter)
        {
            var response = await GetResponseAsync(restRequest);

            return DeserializeObject<T>(response.Content, jsonConverter);
        }
        public async Task<List<Alert>> GetAlertsAsync()
        {
            var request = PrepareAuthRequest(Endpoints.Alerts);
            return await GetResourceAsync<List<Alert>>(request, new AlertsConverter());
        }

        public async Task<PlatformStatus> GetPlatformStatusAsync()
        {
            var response = await GetResponseAsync(new RestRequest(Endpoints.PlatformStatus));
            return DeserializeObject<PlatformStatus>(response.Content, new PlatformStatusConverter());
        }

        public async Task<List<Wallet>> GetWalletsAsync()
        {                       
            var request = PrepareAuthRequest(Endpoints.Wallets);
            return await GetResourceAsync<List<Wallet>>(request, new WalletsConverter());
        }

        private RestRequest PrepareAuthRequest(string apiPath)
        {
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

            return request;
        }
    }   
}