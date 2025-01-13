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

        [HttpGet("clients/{userId}/sprints/{sprintId}")]
        public async Task<ActionResult<SprintResponse>> GetSprintById(Guid userId, Guid sprintId)
        {
            try
            {
                var sprint = await _sprintService.GetSprintByIdAsync(userId, sprintId);
                return Ok(sprint);
            }
            catch (UnauthorizedAccessException)
            {
                return NotFound();
            }
        }

        [HttpGet("clients/{userId}/sprints")]
        public async Task<ActionResult<IList<SprintResponse>>> GetUserSprints(Guid userId)
        {
            var sprints = await _sprintService.GetSprintsByUserIdAsync(userId);
            return Ok(sprints);
        }
    }
}
