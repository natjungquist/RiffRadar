namespace RiffRadar.Models.Data.Responses
{
    public class UserPlaylists
    {
        public string href { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }
        public Playlist[] items { get; set; }

    }
}
