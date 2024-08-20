using RiffRadar.Models.Data;
using RiffRadar.Models.Data.Responses;
using System.ComponentModel.DataAnnotations;

namespace RiffRadar.Models.DataTransfer
{ public class HomeIndexView
    {
        public UserProfile? UserProfile { get; set; }
        public UserPlaylists? UserPlaylists { get; set; }
        public UserTopTracks? UserTopTracks { get; set; }
        public int? TotalGenres { get; set; }
        public List<string>? AllGenres { get; set; }
        public List<string>? TopGenres { get; set; }
        public List<string>? FilteredTracks { get; set; }
    }
   
}
