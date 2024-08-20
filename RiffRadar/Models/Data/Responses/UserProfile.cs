using static RiffRadar.Models.Data.Responses.Shared;

namespace RiffRadar.Models.Data.Responses
{
    public class UserProfile
    {
        public string country { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public Explicit_Content explicit_content { get; set; }
        public External_Urls external_urls { get; set; }
        public Followers followers { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public Image[] images { get; set; }
        public string product { get; set; }
        public string type { get; set; }
        public string uri { get; set; }

        public class Explicit_Content
        {
            public bool filter_enabled { get; set; }
            public bool filter_locked { get; set; }
        }
    }
}
