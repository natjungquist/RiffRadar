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
using Microsoft.AspNetCore.Diagnostics;

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
            User user = new()
            {
                AuthAccessToken = token
            };
            user.UserProfile = await _spotifyService.GetProfile(user.AuthAccessToken);
            user.UserPlaylists = await _spotifyService.GetUserPlaylists(user.UserProfile.id, user.AuthAccessToken);
            user.UserTopTracks = await _spotifyService.GetTopTracks(user.AuthAccessToken, 0);
            user.TracksDict = await _genreManager.GetToptracksDict(user, _spotifyService);
            user.Genres = await _genreManager.GetTopGenres(user.TracksDict);
            _memory.Set("User", user, TimeSpan.FromMinutes(60));

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Redirect here after collecting user information.
        /// </summary>
        [Route("Home")]
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
                TotalGenres = user.Genres.Count,
                Genres = user.Genres,
                TracksDict = user.TracksDict
            };
            return View(homeIndexView);
        }

        /// <summary>
        ///     Action method occurs when the user hits "Submit" after selecting genres.
        /// </summary>
        [HttpPost]
        public IActionResult FilterByGenre(List<string> selectedGenres)
        {
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }

            // ChainingTable track : [genres]
            user.FilteredTracksDict = _genreManager.FilterByGenres(selectedGenres, user.TracksDict);

            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                TotalGenres = user.Genres.Count,
                Genres = user.Genres,
                FilteredTracksDict = user.FilteredTracksDict
            };
            return View("Index", homeIndexView);
        }

        /// <summary>
        ///     Call the Spotify Web API to create a playlist based on the tracks displayed on screen.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePlaylist(string playlistName)
        {
            // validate playlist name
            if (string.IsNullOrEmpty(playlistName)) {
                ViewBag.ErrorTitle = "Invalid input.";
                ViewBag.ErrorMessage = "The playlist cannot be created. It must have a name.";
                ErrorViewModel err = new()
                {
                    RequestId = "404"
                };
                return View("Error", err);
            }

            //validate session
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }

            // check which tracks to look at
            List<string> trackUris = new();
            ChainingTable userTracks = new();
            if (user.FilteredTracksDict != null)
            {
                userTracks = user.FilteredTracksDict;
            }
            else if (user.TracksDict != null)
            {
                userTracks = user.TracksDict;
            }
            foreach (Track track in userTracks.GetKeys())
            {
                trackUris.Add(track.uri);
            }
            Playlist newPlaylist = await _spotifyService.CreatePlaylist(playlistName, trackUris, user.UserProfile.id, user.AuthAccessToken);
            user.UserPlaylists = await _spotifyService.GetUserPlaylists(user.UserProfile.id, user.AuthAccessToken);

            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                TotalGenres = user.Genres.Count,
                Genres = user.Genres
            };
            if (user.FilteredTracksDict != null)
            {
                homeIndexView.FilteredTracksDict = userTracks;
            } 
            else if (user.TracksDict != null)
            {
                homeIndexView.TracksDict = userTracks;
            }
            return View("Index", homeIndexView);
        }

        /// <summary>
        ///     Reset home to look like initial load: no genres selected, tracks unfiltered.
        /// </summary>
        [HttpPost]
        public IActionResult Reset()
        {
            if (!_memory.TryGetValue("User", out User user))
            {
                return RedirectToAction("RequestSpotifyUserAuthorization", "Auth");
            }
            user.FilteredTracksDict = null;  //clear out previously filtered tracks

            HomeIndexView homeIndexView = new()
            {
                UserProfile = user.UserProfile,
                UserPlaylists = user.UserPlaylists,
                TotalGenres = user.Genres.Count,
                Genres = user.Genres,
                TracksDict = user.TracksDict
            };
            return View("Index", homeIndexView);
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
        [Route("Error")]
        public IActionResult Error()
        {
            var errorDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The path {errorDetails.Path} threw an exception: " +
                $"{errorDetails.Error}");

            return View(new Models.DataTransfer.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
