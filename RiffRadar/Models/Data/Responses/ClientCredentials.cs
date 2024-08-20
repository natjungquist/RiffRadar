namespace RiffRadar.Models.Data.Responses
{
    /// <summary>
    ///    ClientCredentials is the recipient of the response from the request for the access token in RequestClientCredentialsToken from SpotifyAccountService.
    ///    It is created according to the json result of the authentication, sample shown in spotify documentation.
    /// </summary>
    public class ClientCredentials
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }


    }
}
