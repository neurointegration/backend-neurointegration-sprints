using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;
using Api.Extensions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SprintsController : ControllerBase
    {
        private readonly ISprintService _sprintService;

        public SprintsController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }

        [HttpGet("{sprintNumber}")]
        public async Task<ActionResult<SprintResponse>> GetSprintById(long sprintNumber)
        {
            var userId = User.GetUserId();

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

        [HttpGet]
        public async Task<ActionResult<IList<SprintResponse>>> GetUserSprints()
        {
            var userId = User.GetUserId();
            var sprints = await _sprintService.GetSprintsByUserIdAsync(userId);
            return Ok(sprints);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSprint([FromBody] CreateSprintRequest request)
        {
            var userId = User.GetUserId();

            await _sprintService.CreateSprintAsync(userId, request);
            return Ok();
        }
    }
}
