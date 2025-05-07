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
    public class TrainerSprintsController : ControllerBase
    {
        private readonly ISprintService _sprintService;
        private readonly ITrainerService _trainerService;

        public TrainerSprintsController(ISprintService sprintService, ITrainerService trainerService)
        {
            _sprintService = sprintService;
            _trainerService = trainerService;
        }

        [HttpGet("clients/{userId}/sprints/{sprintNumber}")]
        public async Task<ActionResult<SprintResponse>> GetSprintById(long userId, long sprintNumber)
        {
            try
            {
                var trainerId = User.GetUserId();
                if (!await _trainerService.HasAccessAsync(trainerId, userId))
                    return NotFound();
                var sprint = await _sprintService.GetSprintByIdAsync(userId, sprintNumber);
                return Ok(sprint);
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
        }

        [HttpGet("clients/{userId}/sprints")]
        public async Task<ActionResult<IList<SprintResponse>>> GetUserSprints(long userId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var sprints = await _sprintService.GetSprintsByUserIdAsync(userId);
            return Ok(sprints);
        }
    }
}
