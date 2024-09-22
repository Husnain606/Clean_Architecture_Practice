namespace IdentityService.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; } = "hgudhudfgdhguhgudhudfgdhgudhudfgdggdhudfgdgg";
        public int Expiry { get; set; } = 10;
    }
}
