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
        
        //public IRestResponse Execute(RestRequest request) //where T : new()
        //{
        //    var client = new RestClient();
        //    client.BaseUrl = new Uri(BaseUrl);
        //    //client.Authenticator = new HttpBasicAuthenticator(_accountSid, _secretKey);
        //    //request.AddParameter("AccountSid", _accountSid, ParameterType.UrlSegment); // used on every request
        //    var response = client.Execute(request);

        //    //JsonDeserializer deserial = new JsonDeserializer();
        //    //var messageList = deserial.Deserialize<PlatformStatus>(response);
        //    //JObject jsonResponse = (JObject) JsonConvert.DeserializeObject(response.Content);
        //    //var messageList = JsonConvert.DeserializeObject<PlatformStatus>(jsonResponse["messages"].ToString()‌​);

        //    if (response.ErrorException != null)
        //    {
        //        const string message = "Error retrieving response.  Check inner details for more info.";
        //        var bitfinexException = new ApplicationException(message, response.ErrorException);
        //        throw bitfinexException;
        //    }
        
        //    return response;//.Data;
        //}

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
}