namespace SMS.Common.Responses
{
    public class AuthenticationResponse
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public string Token { get; set; } = null!;
      
    }
}
