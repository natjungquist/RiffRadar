using System.Text;
using System.Text.Json;
using RiffRadar.Models.Services.Interfaces;
using RiffRadar.Models.Data.Responses;

namespace RiffRadar.Models.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;
        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ClientCredentialsTokenRequest(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            Dictionary<string, string> authParameters = new()
            {
                {"grant_type", "client_credentials"}
            };
            request.Content = new FormUrlEncodedContent(authParameters);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            //handle the api response
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var clientCredentials = await JsonSerializer.DeserializeAsync<ClientCredentials>(responseStream);
            if (clientCredentials != null)
            {
                return clientCredentials.access_token;
            }
            else
            {
                throw new Exception("client credentials is null");
            }
        }

        public async Task<string> AuthorizationCodeTokensRequest(string clientId, string clientSecret, string code, string redirectUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            Dictionary<string, string> authParameters = new()
            {
                {"grant_type", "authorization_code"},
                {"code", code},
                {"redirect_uri", redirectUri}
            };
            request.Content = new FormUrlEncodedContent(authParameters);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            //handle the api response
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var authorizationCode = await JsonSerializer.DeserializeAsync<AuthorizationCode>(responseStream);
            if (authorizationCode != null && authorizationCode.access_token != null)
            {
                return authorizationCode.access_token;
            }
            else
            {
                throw new Exception("authorization code is null");
            }
        }

        public async Task<string> RefreshAccessTokenRequest(string clientId, string clientSecret, string refreshToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            Dictionary<string, string> authParameters = new()
            {
                {"grant_type", "refresh_token"},
                {"refresh_token", refreshToken}
            };
            request.Content = new FormUrlEncodedContent(authParameters);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            //handle the api response
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var refresh = await JsonSerializer.DeserializeAsync<AuthorizationCode>(responseStream);
            if (refresh != null && refresh.access_token != null)
            {
                return refresh.access_token;
            }
            else
            {
                throw new Exception("error refreshing the access token");
            }
        }
    }
}
