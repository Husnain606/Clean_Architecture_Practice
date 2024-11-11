namespace SMS.Application.Services.Account.Dto
{
    public class UpdateRoleClaimDto
    {
        public RoleClaimDto OldClaim { get; set; }
        public RoleClaimDto NewClaim { get; set; }
    }
}
