namespace Data.Repositories
{           
    public interface ITrainerRepository
    {
        Task<ApplicationUser?> GetClientAsync(long trainerId, long clientId);
        Task<IList<ApplicationUser>> GetClientsAsync(long trainerId);
        Task<TrainerComment?> GetCommentAsync(long trainerId, long clientId);
        Task<ApplicationUser?> GetTrainerAsync(long clientId);
        Task<bool> UpdateCommentAsync(TrainerComment comment);
    }
}
