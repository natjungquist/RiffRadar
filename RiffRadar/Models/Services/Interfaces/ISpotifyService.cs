using RiffRadar.Models.Data.Responses;

namespace RiffRadar.Models.Services.Interfaces
{
    public interface ISpotifyService
    {
        Task<UserProfile> GetProfile(string accessToken);
        Task<UserTopTracks> GetTopTracks(string accessToken, int offset);
        Task<Artist> GetArtist(string accessToken, string artistId);

        Task<UserPlaylists> GetUserPlaylists(string userid, string accessToken);

    }
}
