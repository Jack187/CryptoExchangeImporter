using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaseApiClient
{
    public abstract class BaseRestClient
    {
        protected readonly string ApiKey;
        protected readonly string SecretKey;

        public readonly RestClient _client;
        private readonly CancellationToken _cancellationToken;

        protected BaseRestClient(string apiKey, string secretKey, string baseUrl)
        {
            ApiKey = apiKey;
            SecretKey = secretKey;

            _client = new RestClient(baseUrl);
            _cancellationToken = new CancellationTokenSource().Token;
        }

        protected virtual async Task<IRestResponse> GetResponseAsync(IRestRequest request)
        {
            IRestResponse response = null;

            switch (request.Method)
            {
                case Method.GET:
                    response = await _client.ExecuteGetTaskAsync(request, _cancellationToken);
                    break;
                case Method.POST:
                    response = await _client.ExecutePostTaskAsync(request, _cancellationToken);
                    break;
            }

            if (!string.IsNullOrEmpty(response.ErrorMessage))
                throw new Exception(response.ErrorMessage);

            return response;
        }
    }
}