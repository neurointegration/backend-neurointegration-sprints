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
    public class TrainerSprintAnswersController : ControllerBase
    {
        private readonly ISprintAnswerService _sprintAnswerService;
        private readonly ITrainerService _trainerService;

        public TrainerSprintAnswersController(ISprintAnswerService sprintAnswerService, ITrainerService trainerService)
        {
            _sprintAnswerService = sprintAnswerService;
            _trainerService = trainerService;
        }

        [HttpGet("clients/{userId}/standup/sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<StandupResponse>>> GetStandup(long userId, long sprintNumber)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var standup = await _sprintAnswerService.GetStandupAsync(userId, sprintNumber);
            return Ok(standup);
        }

        [HttpGet("clients/{userId}/standups")]
        public async Task<ActionResult<IList<StandupResponse>>> GetAllStandups(long userId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var standups = await _sprintAnswerService.GetAllStandupsAsync(userId);
            return Ok(standups);
        }

        [HttpGet("clients/{userId}/reflection/sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<ReflectionResponse>>> GetReflection(long userId, long sprintNumber)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var reflection = await _sprintAnswerService.GetReflectionAsync(userId, sprintNumber);
            return Ok(reflection);
        }

        [HttpGet("clients/{userId}/reflections")]
        public async Task<ActionResult<IList<ReflectionResponse>>> GetAllReflections(long userId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var reflections = await _sprintAnswerService.GetAllReflectionsAsync(userId);
            return Ok(reflections);
        }
    }
}
