namespace Data.Repositories
{
    public interface ISprintRepository
    {
        Task<Sprint?> GetSprintAsync(long userId, long sprintNumber);
        Task CreateSprintAsync(Sprint sprint);
        Task UpdateSprintAsync(Sprint sprint);
        Task<IList<Sprint>> GetSprintsByUserIdAsync(long userId);
    }
}
