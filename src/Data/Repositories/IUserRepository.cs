namespace Data.Repositories
{   
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(long userId);
        Task<ApplicationUser?> GetUserByUserNameAsync(string userName);
        Task<ApplicationUser> UpsertAsync(ApplicationUser user);
    }
}
