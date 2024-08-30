using RiffRadar.Models.Data;
using RiffRadar.Models.Data.Responses;

namespace RiffRadar.Models.Services.Interfaces
{
    public interface IGenreManager
    {
        Task<ChainingTable> GetToptracksDict(User user, ISpotifyService spotifyService);
        Task<List<string>> GetTopGenres(ChainingTable tracksDict);
        ChainingTable FilterByGenres(List<string> selectedGenres, ChainingTable genresDict);
        (Track, string) GetMostPopularTrack(ChainingTable tracksDict);
    }
}
