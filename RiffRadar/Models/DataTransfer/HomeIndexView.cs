using RiffRadar.Models.Data;
using RiffRadar.Models.Data.Responses;
using System.ComponentModel.DataAnnotations;

namespace RiffRadar.Models.DataTransfer
{ public class HomeIndexView
    {
        public UserProfile? UserProfile { get; set; }
        public UserPlaylists? UserPlaylists { get; set; }
        public int? TotalGenres { get; set; }
        public List<string>? Genres { get; set; }
        public ChainingTable? TracksDict { get; set; }
        public ChainingTable? FilteredTracksDict { get; set; }
    }
   
}
