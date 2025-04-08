using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/Trainer")]
    [Authorize(Roles = "Trainer")]
    public class TrainerSprintsController : ControllerBase
    {
        private readonly ISprintService _sprintService;

        public TrainerSprintsController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }

        [HttpGet("clients/{userId}/sprints/{sprintNumber}")]
        public async Task<ActionResult<SprintResponse>> GetSprintById(long userId, long sprintNumber)
        {
            try
            {
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
            var sprints = await _sprintService.GetSprintsByUserIdAsync(userId);
            return Ok(sprints);
        }
    }
}
