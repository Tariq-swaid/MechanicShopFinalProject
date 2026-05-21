namespace MechanicShop.Client.Identity
{
    public class TokenResponse
    {

        public string? AccessToken { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public string? RefreshToken { get; set; }
    }
}
