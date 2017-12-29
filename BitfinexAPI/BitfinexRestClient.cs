using Newtonsoft.Json;
using System;
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
        private const string BaseUrl = "https://api.bitfinex.com/v2";

        public BitfinexRestClient(string apiKey, string secretKey) : 
            base(apiKey, secretKey, BaseUrl)
        {
        }
        
        public static T DeserializeObject<T>(string json, JsonConverter converter)
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

        protected override async Task<IRestResponse> GetResponseAsync(IRestRequest request)//, CancellationToken token)
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
            var request = new RestRequest("platform/status");

            var response = await GetResponseAsync(request);

            return DeserializeObject<PlatformStatus>(response.Content, new PlatformStatusConverter());
        }
    }


    /*
     string key = "APIKEY";
string secret = "APISECRET";

string apiPath = "v2/auth/r/alerts";
            

DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
long nonce = (long)Math.Floor(diff.TotalMilliseconds);

var rawBody = JsonConvert.SerializeObject(new
{
    type = "price"
});

string signature = $"/api/{apiPath}{nonce}{rawBody}";

var hmac = new HMACSHA384(Encoding.UTF8.GetBytes(secret));
byte[] k = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature));
string signatureString = string.Concat(k.Select(b => b.ToString("X2")).ToArray()).ToLower();

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.TryAddWithoutValidation("bfx-nonce", nonce.ToString());
client.DefaultRequestHeaders.TryAddWithoutValidation("bfx-apikey", key);
client.DefaultRequestHeaders.TryAddWithoutValidation("bfx-signature", signatureString);

var response = await client.PostAsync($"https://api.bitfinex.com/{apiPath}", new StringContent(rawBody, Encoding.UTF8, "application/json"));
var responseString = await response.Content.ReadAsStringAsync();
     */
}