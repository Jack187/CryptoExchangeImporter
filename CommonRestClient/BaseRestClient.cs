using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using RestSharp;

namespace CommonRestClient
{
    public abstract class BaseRestClient
    {
        private readonly string _apiKey;
        private readonly string _secretKey;

        private readonly RestClient _client;
        private readonly CancellationToken _cancellationToken;

        protected BaseRestClient(string apiKey, string secretKey, string baseUrl)
        {
            _apiKey = apiKey;
            _secretKey = secretKey;

            _client = new RestClient(baseUrl);
            _cancellationToken = new CancellationTokenSource().Token;
        }

        protected virtual async Task<IRestResponse> GetResponseAsync(IRestRequest request)
        {
            var response = await _client.ExecuteGetTaskAsync(request, _cancellationToken);

            if (!string.IsNullOrEmpty(response.ErrorMessage))
                throw new Exception(response.ErrorMessage);

            return response;
        }
    }
}