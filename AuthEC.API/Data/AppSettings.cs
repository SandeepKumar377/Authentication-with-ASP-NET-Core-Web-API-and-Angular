namespace AuthEC.API.Data
{
    public class AppSettings
    {
        public string? JWTSecret { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
