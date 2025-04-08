using Service.Dto;

namespace Service
{
    public interface ITrainerService
    {
        Task<ClientResponse?> GetClientAsync(long trainerId, long clientId);
        Task<IList<ClientResponse>> GetClientsAsync(long userId);
    }
}
