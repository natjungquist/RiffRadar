using Microsoft.AspNetCore.Mvc;
using RiffRadar.Models.Services.Interfaces;
using RiffRadar.Models.Data;
using System.Diagnostics;

namespace RiffRadar.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISpotifyAccountService _spotifyAccountService;
        private readonly IConfiguration _configuration;
        private string redirectUri = "https://localhost:44350/auth/callback";
        public AuthController(IConfiguration configuration, ISpotifyAccountService spotifyAccountService)
        {
            _configuration = configuration;
            _spotifyAccountService = spotifyAccountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        ///     This action is invoked when the user presses a button to log in to their Spotify account.
        ///     It redirects users to the Spotify authorization page.
        /// </summary>
        public IActionResult RequestSpotifyUserAuthorization()
        {
            string clientId = _configuration["Spotify:ClientId"];
            var scopes = new List<string> { Scopes.PlaylistReadPrivate, Scopes.PlaylistModifyPrivate, Scopes.UserReadPrivate, Scopes.UserTopRead, Scopes.UserLibraryModify, Scopes.UserLibraryRead };
            var scope = string.Join(" ", scopes);

            //string state = 

            var authorizationUrl = $"https://accounts.spotify.com/authorize?client_id={clientId}" +
            $"&response_type=code" +
            $"&redirect_uri={redirectUri}" +
            $"&scope={scope}"; 
            //+
            //$"&show_dialog=true";
            return Redirect(authorizationUrl.ToString());
        }

        /// <summary>
        ///     This action is called any time there is a callback.
        ///     It will redirect the user to the Home Index page.
        /// </summary>
        [Route("auth/callback")]
        public async Task<IActionResult> Callback(string code, string state, string error)
        {
            try
            {
                if (!string.IsNullOrEmpty(error))
                {
                    return View("Error with callback");
                }

                if (!string.IsNullOrEmpty(code))
                {
                    string clientId = _configuration["Spotify:ClientId"];
                    string clientSecret = _configuration["Spotify:ClientSecret"];
                    var token = await _spotifyAccountService.AuthorizationCodeTokensRequest(clientId, clientSecret, code, redirectUri);
                    TempData["Token"] = token;
                }

                return RedirectToAction("InitialLoad", "Home");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
