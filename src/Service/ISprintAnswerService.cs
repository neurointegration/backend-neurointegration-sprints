using Service.Dto;

namespace Service
{
    public interface ISprintAnswerService
    {
        Task<IList<StandupResponse>> GetStandupAsync(long userId, long sprintNumber);
        Task<IList<StandupResponse>> GetAllStandupsAsync(long userId);
        Task<IList<ReflectionResponse>> GetReflectionAsync(long userId, long sprintNumber);
        Task<IList<ReflectionResponse>> GetAllReflectionsAsync(long userId);
    }
}
