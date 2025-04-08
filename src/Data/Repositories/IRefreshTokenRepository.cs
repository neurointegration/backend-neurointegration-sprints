namespace Data.Repositories
{   
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> CreateAsync(RefreshToken token);
        Task<bool> IsOkAsync(string tokenHash, long userId);
        Task<bool> RemoveAsync(string tokenHash);
        Task<bool> RemoveIfOkAsync(string tokenHash, long userId);
    }
}
