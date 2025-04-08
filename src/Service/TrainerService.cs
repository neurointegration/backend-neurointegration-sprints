using Data;
using Data.Repositories;
using Service.Dto;

namespace Service
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerRepository _trainerRepository;

        public TrainerService(ITrainerRepository trainerRepository)
        {
            _trainerRepository = trainerRepository;
        }

        public async Task<IList<ClientResponse>> GetClientsAsync(long userId)
        {
            var clients = await _trainerRepository.GetClientsAsync(userId);

            return clients.Select(MapToClientResponse).ToList();
        }

        public async Task<ClientResponse?> GetClientAsync(long trainerId, long clientId)
        {
            var client = await _trainerRepository.GetClientAsync(trainerId, clientId);

            if (client == null)
                return null;

            return MapToClientResponse(client);
        }

        private ClientResponse MapToClientResponse(ApplicationUser user)
        {
            return new ClientResponse
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.Name,
                AboutMe = user.AboutMe,
                PhotoUrl = user.PhotoUrl
            };
        }
    }
}
