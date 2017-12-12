using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Test
{
    public class BitfinexRestClient
    {
        private RestClient _client;
        private string _baseUrl = "https://api.bitfinex.com/v2";

        public CancellationToken CancellationToken { get; set; }
        public BitfinexRestClient()
        {
            _client = new RestClient(_baseUrl);
            CancellationToken = new CancellationTokenSource().Token;
        }

        private async Task<IRestResponse> GetResponseAsync(IRestRequest request, CancellationToken token)
        {
            var response = await _client.ExecuteTaskAsync(request, token);
            if (!string.IsNullOrEmpty(response.ErrorMessage)) throw new Exception(response.ErrorMessage);

            //BitfinexException exception = null;
            //try
            //{
            //    exception = JsonConvert.DeserializeObject<BitfinexException>(response.Content, new ExceptionResultConverter());
            //}
            //catch (Exception)
            //{
            //    // ignored
            //}

            //if (exception != null) throw new Exception($"({exception.ErrorCode}) {exception.Message}");

            return response;
        }

        public async void Test()
        {
            var request = new RestRequest("platform/status", Method.GET);
            var response = await GetResponseAsync(request, CancellationToken);
        }
    }
}
