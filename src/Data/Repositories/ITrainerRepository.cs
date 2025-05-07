
namespace Data.Repositories
{           
    public interface ITrainerRepository
    {
        Task<ApplicationUser?> GetClientAsync(long trainerId, long clientId);
        Task<IList<ApplicationUser>> GetClientsAsync(long trainerId);
        Task<TrainerComment?> GetCommentAsync(long trainerId, long clientId);
        Task<ApplicationUser?> GetTrainerAsync(long clientId);
        Task<bool> HasAccessAsync(long treinerId, long clientId);
        Task<bool> HasProjectAccessAsync(long trainerId, long clientId, Guid clientProjectId);
        Task<bool> HasTaskAccessAsync(long trainerId, long clientId, Guid clientTaskId);
        Task<bool> UpdateCommentAsync(TrainerComment comment);
    }
}
