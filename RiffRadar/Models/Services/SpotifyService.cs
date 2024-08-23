using RiffRadar.Models.Data.Responses;
using RiffRadar.Models.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace RiffRadar.Models.Services
{
    public class SpotifyService : ISpotifyService
    {
        private string uriBase = "https://api.spotify.com/v1";
        private readonly HttpClient _httpClient;
        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient; //will be injected at runtime
        }

        public async Task<UserProfile> GetProfile(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = $"{uriBase}/me";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var profile = await JsonSerializer.DeserializeAsync<UserProfile>(responseStream);
            if (profile != null)
            {
                return profile;
            }
            else
            {
                throw new Exception("error retrieving user profile");
            }
        }

        public async Task<Artist> GetArtist(string accessToken, string artistId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = $"{uriBase}/artists/{artistId}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var artist = await JsonSerializer.DeserializeAsync<Artist>(responseStream);
            if (artist != null)
            {
                return artist;
            }
            else
            {
                throw new Exception("error retrieving artist");
            }
        }

        public async Task<UserTopTracks> GetTopTracks(string accessToken, int offset)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                   "Bearer", accessToken);

            //can return 50 top tracks at a time
            //DEFAULT is time_range=medium_term, which is approximately the last 6 months

            string uri = $"{uriBase}/me/top/tracks?limit=50&offset={offset}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var topTracks = await JsonSerializer.DeserializeAsync<UserTopTracks>(responseStream);
            if (topTracks != null)
            {
                return topTracks;
            }
            else
            {
                throw new Exception("error retrieving top tracks");
            }
        }

        public async Task<UserPlaylists> GetUserPlaylists(string userid, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = $"{uriBase}/users/{userid}/playlists";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var playlists = await JsonSerializer.DeserializeAsync<UserPlaylists>(responseStream);
            if (playlists != null)
            {
                return playlists;
            }
            else
            {
                throw new Exception("error retrieving user playlists");
            }
        }

        public async Task<Playlist> createPlaylist(string playlistName, List<Track> tracks, string userId, string accessToken)
        {
            Playlist newPlaylist = await createEmptyPlaylist(playlistName, userId, accessToken);
            return newPlaylist;
        }

        public async Task<Playlist> createEmptyPlaylist(string playlistName, string userId, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                "Bearer", accessToken);

            string uri = $"{uriBase}/users/{userId}/playlists";

            var requestBody = new
            {
                name = playlistName,
                description = "New playlist description",  // You can customize this
                @public = false                            // Use @public to escape the reserved keyword
            };
            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var playlist = await JsonSerializer.DeserializeAsync<Playlist>(responseStream);
            if (playlist != null)
            {
                return playlist;
            }
            else
            {
                throw new Exception("error creating playlist");
            }
        }

        public async Task<string> addTracks(string playlistId)
        {
            string snapshot_id = "";
            return snapshot_id;
        }

    }
}
