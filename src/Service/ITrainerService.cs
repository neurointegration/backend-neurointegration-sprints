using Service.Dto;

namespace Service
{
    public interface ITrainerService
    {
        Task<ClientResponse?> GetClientAsync(long trainerId, long clientId);
        Task<IList<ClientResponse>> GetClientsAsync(long userId);
        Task<bool> HasAccessAsync(long treinerId, long clientId);
        Task<bool> HasProjectAccessAsync(long trainerId, long clientId, Guid clientProjectId);
        Task<bool> HasTaskAccessAsync(long trainerId, long clientId, Guid clientTaskId);
    }
}
