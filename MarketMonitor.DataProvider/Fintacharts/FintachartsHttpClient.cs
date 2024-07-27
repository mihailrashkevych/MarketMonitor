using MarketMonitor.RI.Contracts.Clients;
using MarketMonitor.RI.Contracts.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MarketMonitor.DataProviders.Fintacharts
{
    public class FintachartsHttpClient : IFintachartsHttpClient
    {
        private readonly ILogger<FintachartsHttpClient> _logger;
        private readonly IConfiguration _configuration;

        private AuthentificationInfo _authentificationInfo;
        private DateTime _expirationTime;
        //private DateTime _refreshExpirationTime = DateTime.MaxValue;

        public FintachartsHttpClient(ILogger<FintachartsHttpClient> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string?> GetTokenAsync()
        {
            if (_expirationTime < DateTime.UtcNow)
            {
                return await GetFreshToken();

                //refresh_tocken does not working, so ignoring!
                //if (_refreshExpirationTime > DateTime.UtcNow)
                //{
                //    return await GetFreshToken();
                //}
                //return _authentificationInfo.RefreshToken;
            }

            return _authentificationInfo.AccessToken;
        }

        public async Task<RI.Contracts.Entities.Instruments?> GetInstrumentsAsync(string? provider = null, string? currency = null, string? symbol = null, string? id = null)
        {
            IConfigurationSection configurationSection = _configuration.GetSection("AuthentificationCredentials");
            string? uriValue = configurationSection.GetSection("BaseUri").Value;

            if (uriValue == null) return null;

            Uri baseAddress = new Uri(uriValue);

            using var httpClient = new HttpClient { BaseAddress = baseAddress };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync());

            var uriParameters = new Dictionary<string, string>();
            if(provider != null) uriParameters.Add(nameof(provider), provider);
            if (currency != null) uriParameters.Add(nameof(currency), currency);
            if (symbol != null) uriParameters.Add(nameof(symbol), symbol);
            if (id != null) uriParameters.Add(nameof(id), id);

            string subBaseUri = "/api/instruments/v1/instruments";
            var combinedUri = QueryHelpers.AddQueryString(subBaseUri, uriParameters);
            
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(combinedUri);

                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();

                Instruments instruments = JsonSerializer.Deserialize<Instruments>(result);

                if (instruments == null) return null;

                return instruments;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message, $"{nameof(GetFreshToken)}");
                return null;
            }
        }

        private async Task<string?> GetFreshToken()
        {
            IConfigurationSection configurationSection = _configuration.GetSection("AuthentificationCredentials");

            string? uriValue = configurationSection.GetSection("BaseUri").Value;

            if (uriValue == null) return null;

            Uri baseAddress = new Uri(uriValue);

            using HttpClient httpClient = new HttpClient { BaseAddress = baseAddress };
            List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(configurationSection.GetSection("grant_type").Key, configurationSection.GetSection("grant_type").Value),
                    new KeyValuePair<string, string>(configurationSection.GetSection("client_id").Key, configurationSection.GetSection("client_id").Value),
                    new KeyValuePair<string, string>(configurationSection.GetSection("username").Key, configurationSection.GetSection("username").Value),
                    new KeyValuePair<string, string>(configurationSection.GetSection("password").Key, configurationSection.GetSection("password").Value)
                };

            HttpContent content = new FormUrlEncodedContent(formData);

            try
            {
                HttpResponseMessage response = 
                    await httpClient.PostAsync($"/identity/realms/{configurationSection.GetSection("realm").Value}/protocol/openid-connect/token", content);

                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();

                _authentificationInfo = JsonSerializer.Deserialize<AuthentificationInfo>(result);

                if (_authentificationInfo == null) return null;

                _expirationTime = DateTime.UtcNow.AddSeconds(_authentificationInfo.ExpiresIn);
                return _authentificationInfo.AccessToken;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message, $"{nameof(GetFreshToken)}");
                return null;
            }
        }
    }
}