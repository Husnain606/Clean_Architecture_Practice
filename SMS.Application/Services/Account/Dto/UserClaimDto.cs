namespace SMS.Application.Services.Account.Dto
{
    public class UserClaimDto
    {
        public Guid UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
