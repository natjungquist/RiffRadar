using System.ComponentModel.DataAnnotations;
using RiffRadar.Models.Data.Responses;

namespace RiffRadar.Models.Data
{
    public class User
    {
        [Required]
        public string? AuthAccessToken { get; set; }

        public string? UserId { get; set; }

        public UserProfile? UserProfile { get; set; }

        public UserTopTracks? UserTopTracks { get; set; }

        public UserTopArtists? UserTopArtists { get; set; }

        public UserPlaylists? UserPlaylists { get; set; }
        public ChainingTable? TracksDict { get; set; }
        public List<string> Genres { get; set; }
    }
}
