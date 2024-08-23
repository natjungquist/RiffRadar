using Microsoft.AspNetCore.Mvc;
using RiffRadar.Models;
using System.Diagnostics;
using RiffRadar.Models.Services.Interfaces;
using RiffRadar.Models.Data;
using RiffRadar.Models.DataTransfer;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using Microsoft.AspNetCore.Http.Features;
using RiffRadar.Models.Data.Responses;
using System.Runtime.ConstrainedExecution;

namespace RiffRadar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISpotifyService _spotifyService;
        private readonly IGenreManager _genreManager;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memory;

        public HomeController(IConfiguration configuration, ISpotifyService spotifyService, IGenreManager genreManager, ILogger<HomeController> logger, IMemoryCache memory)
        {
            _configuration = configuration;
            _spotifyService = spotifyService;
            _genreManager = genreManager;
            _logger = logger;
            _memory = memory;
        }

        /// <summary>
        ///     Redirect here after OAuth2. Collect initial user information.
        /// </summary>
        public async Task<IActionResult> InitialLoad()
        {
            var token = TempData["Token"] as string;
            if (token == null)
            {
                throw new Exception("Access token is null");
            }

            User user = new User();
            user.AuthAccessToken = token;
            user.UserProfile = await _spotifyService.GetProfile(user.AuthAccessToken);
            //user.UserPlaylists = await _spotifyService.GetUserPlaylists(clientId, clientSecret, userid, user.AuthAccessToken);
            user.UserTopTracks = await _spotifyService.GetTopTracks(user.AuthAccessToken, 0);
            user.TracksDict = await _genreManager.GetToptracksDict(user, _spotifyService);
            user.Genres = await _genreManager.GetTopGenres(user.TracksDict);
            _memory.Set("User", user, TimeSpan.FromMinutes(60));

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Redirect here after collecting user information.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }

            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                UserTopTracks = user.UserTopTracks,
                TotalGenres = user.Genres.Count,
                AllGenres = user.Genres
            };
            return View(homeIndexView);
        }

        /// <summary>
        ///     Action method occurs when the user hits "Submit" after selecting genres.
        /// </summary>
        public IActionResult FilterByGenre(List<string> selectedGenres)
        {
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }

            ChainingTable filteredHt = _genreManager.FilterByGenres(selectedGenres, user.TracksDict);
            List<string> filteredTracks = new();
            foreach (Track track in filteredHt.GetKeys())
            {
                filteredTracks.Add(track.name);
            }
            
            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                UserTopTracks = user.UserTopTracks,
                FilteredTracks = filteredTracks
            };
            return View("Index", homeIndexView);
        }

        public IActionResult CreatePlaylist(List<string> tracks, string playlistName) { 
            
            return View("Index"); 
        }

        /// <summary>
        ///     Action method occurs when user hits Logout.
        /// </summary>
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Auth");
        }

        /// <summary>
        ///     Error handling.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.DataTransfer.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}