namespace IdentityService.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public int Expiry { get; set; }
    }
}
