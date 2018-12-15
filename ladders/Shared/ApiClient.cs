using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ladders.Shared
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetAsync(string path);
        Task<HttpResponseMessage> PostAsync(string path, object payload);
        Task<HttpResponseMessage> DeleteAsync(string path);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;
        private readonly IConfigurationSection _appConfig;
        private readonly DiscoveryCache _discoveryCache;
        private readonly ILogger _logger;

        public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ApiClient> log)
        {
            _appConfig = configuration.GetSection("ladders");
            _discoveryCache = new DiscoveryCache(_appConfig.GetValue<string>("GatekeeperUrl"));
            _client = httpClientFactory.CreateClient("LadderClient");
            _logger = log;
        }

        private async Task<string> GetTokenAsync()
        {
            var discovery = await _discoveryCache.GetAsync();
            if (discovery.IsError)
            {
                _logger.LogError(discovery.Error);
                throw new ApiClientException("Couldn't read discovery document.");
            }

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = _appConfig.GetValue<string>("ClientId"),
                ClientSecret = _appConfig.GetValue<string>("ClientSecret"),

                // The ApiResourceName of the resources you want to access.
                // Other valid values might be `comms`, `health_data_repository`, etc.
                // Ask in #dev-gatekeeper for help
                Scope = "gatekeeper comms booking_facilities"
            };
            var response = await _client.RequestClientCredentialsTokenAsync(tokenRequest);
            if (!response.IsError) return response.AccessToken;

            _logger.LogError(response.Error);
            throw new ApiClientException("Couldn't retrieve access token.");
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            _client.SetBearerToken(await GetTokenAsync());
            return await _client.GetAsync(uri);
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, object payload)
        {
            _client.SetBearerToken(await GetTokenAsync());
            return await _client.PostAsJsonAsync(uri, payload);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            _client.SetBearerToken(await GetTokenAsync());
            return await _client.DeleteAsync(uri);
        }
    }

    public class ApiClientException : Exception
    {
        public ApiClientException(string message) : base(message)
        {
        }
    }
}
