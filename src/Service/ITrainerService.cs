using Service.Dto;

namespace Service
{
    public interface ITrainerService
    {
        Task<ClientResponse?> GetClientAsync(Guid trainerId, Guid clientId);
        Task<IList<ClientResponse>> GetClientsAsync(Guid userId);
    }
}
