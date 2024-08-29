namespace RiffRadar.Models.Data.Responses
{
    public class Shared
    {
        public class External_Urls
        {
            public string spotify { get; set; }
        }

        public class Followers
        {
            public string href { get; set; }

            public int? total { get; set; }
        }

        public class Image
        {
            public string? url { get; set; }
            public int? height { get; set; }
            public int? width { get; set; }
        }

        public class Tracks
        {
            public string href { get; set; }
            public int total { get; set; }
        }

        public class External_Ids
        {
            public string isrc { get; set; }
        }
    }
}
