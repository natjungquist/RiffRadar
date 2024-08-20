namespace RiffRadar.Models.Data.Responses
{
    public class UserTopTracks
    {
        public Track[] items { get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public string href { get; set; }
        public object previous { get; set; }
        public string next { get; set; }
    }
}
