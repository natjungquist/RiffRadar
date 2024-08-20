namespace RiffRadar.Models.Data
{
    public class Scopes
    {
        //Images

        //Spotify connect

        //Playback

        //Playlists
        public const string PlaylistReadCollaborative = "playlist-read-collaborative";
        public const string PlaylistModifyPublic = "playlist-modify-public";
        public const string PlaylistReadPrivate = "playlist-read-private";
        public const string PlaylistModifyPrivate = "playlist-modify-private";

        //Follow

        //Listening history
        public const string UserReadPlaybackPosition = "user-read-playback-position";
        public const string UserTopRead = "user-top-read";
        public const string UserReadRecentlyPlated = "user-read-recently-played";

        //Library
        public const string UserLibraryModify = "user-library-modify";
        public const string UserLibraryRead = "user-library-read";

        //Users
        public const string UserReadEmail = "user-read-email";
        public const string UserReadPrivate = "user-read-private";

        //Open access
        // n/a
    }
}
