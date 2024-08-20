namespace RiffRadar.Models.Services.Interfaces
{
    public interface ISpotifyAccountService
    {
        //the string is wrapped in a Task because we are going to call this method asynchronously
        Task<string> ClientCredentialsTokenRequest(string clientId, string clientSecret);
        Task<string> AuthorizationCodeTokensRequest(string clientId, string clientSecret, string code, string redirectUri);

        Task<string> RefreshAccessTokenRequest(string clientId, string clientSecret, string refreshToken);
    }
}
