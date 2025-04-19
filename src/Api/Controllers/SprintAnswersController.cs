using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;
using Api.Extensions;

namespace Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class SprintAnswersController : ControllerBase
    {
        private readonly ISprintAnswerService _sprintAnswerService;

        public SprintAnswersController(ISprintAnswerService sprintAnswerService)
        {
            _sprintAnswerService = sprintAnswerService;
        }

        [HttpGet("standup/sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<StandupResponse>>> GetStandup(long sprintNumber)
        {
            var userId = User.GetUserId();
            var standup = await _sprintAnswerService.GetStandupAsync(userId, sprintNumber);
            return Ok(standup);
        }

        [HttpGet("standups")]
        public async Task<ActionResult<IList<StandupResponse>>> GetAllStandups()
        {
            var userId = User.GetUserId();
            var standups = await _sprintAnswerService.GetAllStandupsAsync(userId);
            return Ok(standups);
        }

        [HttpGet("reflection/sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<ReflectionResponse>>> GetReflection(long sprintNumber)
        {
            var userId = User.GetUserId();
            var reflection = await _sprintAnswerService.GetReflectionAsync(userId, sprintNumber);
            return Ok(reflection);
        }

        [HttpGet("reflections")]
        public async Task<ActionResult<IList<ReflectionResponse>>> GetAllReflections()
        {
            var userId = User.GetUserId();
            var reflections = await _sprintAnswerService.GetAllReflectionsAsync(userId);
            return Ok(reflections);
        }
    }
}
