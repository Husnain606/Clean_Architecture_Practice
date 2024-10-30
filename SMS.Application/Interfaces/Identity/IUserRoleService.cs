namespace SMS.Application.Interfaces.Identity
{
    public interface IUserRoleService
    {
        Task AssignRoleAsync(string userId, string roleId);
        Task RemoveRoleAsync(string userId, string roleId);
    }
}
