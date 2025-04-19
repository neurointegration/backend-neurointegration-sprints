namespace Data.Repositories
{
    public interface ISprintAnswerRepository
    {
        Task<IList<SprintAnswer>> GetStandupAsync(long userId, long sprintNumber);
        Task<IList<SprintAnswer>> GetAllStandupsAsync(long userId);
        Task<IList<SprintAnswer>> GetReflectionAsync(long userId, long sprintNumber);
        Task<IList<SprintAnswer>> GetAllReflectionsAsync(long userId);
    }
}
