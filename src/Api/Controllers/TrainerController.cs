using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;
using Api.Extensions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/Trainer")]
    [Authorize(Roles = "Trainer")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        [HttpGet("clients")]
        public async Task<ActionResult<IList<ClientResponse>>> GetClients()
        {
            var trainerId = User.GetUserId();

            var clients = await _trainerService.GetClientsAsync(trainerId);
            return Ok(clients);
        }

        [HttpGet("clients/{userId}")]
        public async Task<ActionResult<ClientResponse>> GetClient(Guid userId)
        {
            var trainerId = User.GetUserId();
            var client = await _trainerService.GetClientAsync(trainerId, userId);
            if (client == null)
                return NotFound();
            return Ok(client);
        }
    }
}
