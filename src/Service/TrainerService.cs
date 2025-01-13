using Data;
using Microsoft.EntityFrameworkCore;
using Service.Dto;

namespace Service
{
    public class TrainerService : ITrainerService
    {
        private readonly ApplicationDbContext _context;

        public TrainerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<ClientResponse>> GetClientsAsync(Guid userId)
        {
            var clients = await _context.Users
                .Where(u => u.TrainerId == userId)
                .ToListAsync();

            return clients.Select(MapToClientResponse).ToList();
        }

        public async Task<ClientResponse?> GetClientAsync(Guid trainerId, Guid clientId)
        {
            var client = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == clientId && u.TrainerId == trainerId);

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
                FirstName = user.FirstName,
                LastName = user.LastName,
                AboutMe = user.AboutMe
            };
        }
    }
}
