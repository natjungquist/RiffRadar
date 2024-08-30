using RiffRadar.Models.Data;
using RiffRadar.Models.Data.Responses;
using RiffRadar.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RiffRadar.Models.Services
{
    public class GenreManager : IGenreManager
    {
        public ChainingTable FilterByGenres(List<string> selectedGenres, ChainingTable tracksDict)
        {
            // returns the ChainingTable tracksDict but only with the selectedGenres

            ChainingTable result = new();

            foreach (Track track in tracksDict.GetKeys())
            {
                List<string> tracksGenres = tracksDict.GetValues(track);
                if (tracksGenres.Any(genre => selectedGenres.Contains(genre)))
                {
                    result.Add(track, tracksGenres.ToList());
                }
            }

            return result;
        }

        private Track[] SortByPopularity(Track[] items)
        {
            return items.OrderByDescending(t => t.popularity).ToArray();
        }
        public async Task<ChainingTable> GetToptracksDict(User user, ISpotifyService spotifyService)
        {
            // returns a ChainingTable of the user's top genres based on their top tracks' artist's genres
            // each key is the top track, value is a list of its associated genres

            ChainingTable tracksDict = new();
            Track[] topTracksOrdered = SortByPopularity(user.UserTopTracks.items);
            foreach (Track track in topTracksOrdered)
            {
                foreach (Artist artist in track.artists)
                {
                    Artist thisArtist = await spotifyService.GetArtist(user.AuthAccessToken, artist.id);
                    foreach (string genre in thisArtist.genres)
                    {
                        tracksDict.Add(track, genre);
                    }
                }
            }
            return tracksDict;
        }

        public async Task<List<string>> GetTopGenres(ChainingTable tracksDict)
        {
            // returns a list of the top genres based on the top tracks

            HashSet<string> genresHs = new();

            foreach (Track track in tracksDict.GetKeys())
            {
                foreach (string genre in tracksDict.GetValues(track))
                {
                    genresHs.Add(genre);
                }
            }

            List<string> genres = genresHs.ToList();

            return genres;
        }

        public (Track, string) GetMostPopularTrack(ChainingTable tracksDict)
        {
            foreach (Track track in tracksDict.GetKeys())
            {
                if (track.popularity == 100)
                {
                    return (track, tracksDict.GetValues(track)[0]);
                }
            }
            Track none = new()
            {
                name = "Does not exist"
            };
            return (none, "Does not exist");
        }
    }
}
