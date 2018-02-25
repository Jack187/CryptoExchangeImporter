using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitfinexApi.JsonConverters;
using BitfinexApi.Resources;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BitfinexApi
{
    public partial class BitfinexApiClient
    {
        private const string BaseUrl = "https://api.bitfinex.com/";
        private const string BfxNonce = "bfx-nonce";
        private const string BfxApiKey = "bfx-apikey";
        private const string BfxSignature = "bfx-signature";

        private readonly HttpClient _httpClient;

        private readonly string ApiKey;
        private readonly string SecretKey;

        public BitfinexApiClient(string apiKey, string secretKey, HttpClient httpClient)
        {
            ApiKey = apiKey;
            SecretKey = secretKey;
                        
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypes.ApplicationJson));
        }

        public async Task<PlatformStatus> GetPlatformStatusAsync()
        {
            var response = await _httpClient.GetAsync(Endpoints.PlatformStatus);

            return await GetResourceAsync<PlatformStatus>(response, new PlatformStatusConverter());
        }

        private async Task<T> GetResourceAsync<T>(HttpResponseMessage response, JsonConverter jsonConverter)
        {
            var content = await response.Content.ReadAsStringAsync();

            BitfinexException exception = null;
            try
            {
                exception = JsonConvert.DeserializeObject<BitfinexException>(
                    content, new BitfinexExceptionConverter());
            }
            catch (Exception)
            {
                /*ignored*/
            }

            if (exception != null)
            {
                throw new Exception($"({exception.ErrorCode}) {exception.Message}");
            }

            return JsonConvert.DeserializeObject<T>(content, jsonConverter);
        }

        public async Task<List<Alert>> GetAlertsAsync()
        {
            var request = CreateAuthRequest(Endpoints.Alerts);
            var response = await _httpClient.SendAsync(request);

            return await GetResourceAsync<List<Alert>>(response, new AlertsConverter());
        }

        public async Task<List<Wallet>> GetWalletsAsync()
        {
            var request = CreateAuthRequest(Endpoints.Wallets);
            var response = await _httpClient.SendAsync(request);

            return await GetResourceAsync<List<Wallet>>(response, new WalletsConverter());
        }
        
        private HttpRequestMessage CreateAuthRequest(string endPoint)
        {
            // see example https://gist.github.com/LarsWesselius/2a48ef7d109b895554487e424ffc1041
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            long nonce = (long)Math.Floor(diff.TotalMilliseconds);

            var rawBody = JsonConvert.SerializeObject(new { type = "price" });

            string signature = $"/api/{endPoint}{nonce}{rawBody}";
            var hmac = new HMACSHA384(Encoding.UTF8.GetBytes(SecretKey));
            byte[] k = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature));
            string signatureString = string.Concat(k.Select(b => b.ToString("X2")).ToArray()).ToLower();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endPoint);
            request.Headers.TryAddWithoutValidation(BfxNonce, nonce.ToString());
            request.Headers.TryAddWithoutValidation(BfxApiKey, ApiKey);
            request.Headers.TryAddWithoutValidation(BfxSignature, signatureString);
            request.Content = new StringContent(rawBody, Encoding.UTF8, MediaTypes.ApplicationJson);

            return request;
        }
    }
}