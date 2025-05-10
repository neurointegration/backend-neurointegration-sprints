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
    public class TrainerProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITrainerService _trainerService;

        public TrainerProjectsController(IProjectService projectService, ITrainerService trainerService)
        {
            _projectService = projectService;
            _trainerService = trainerService;
        }

        [HttpGet("clients/{userId}/projects/{projectId}")]
        public async Task<ActionResult<ProjectResponse>> GetProjectById(long userId, Guid projectId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasProjectAccessAsync(trainerId, userId, projectId))
                return NotFound();
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("clients/{userId}/projects/sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<ProjectResponse>>> GetProjectsBySprintId(long userId, long sprintNumber)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var projects = await _projectService.GetProjectsBySprintAsync(userId, sprintNumber);
            return Ok(projects);
        }

        [HttpPost("clients/{userId}/projects")]
        public async Task<ActionResult<ProjectResponse>> CreateProject(long userId, [FromBody] CreateProjectRequest request)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasAccessAsync(trainerId, userId))
                return NotFound();
            var result = await _projectService.CreateProjectAsync(userId, request);
            return Ok(result);
        }

        [HttpPut("clients/{userId}/projects")]
        public async Task<ActionResult<ProjectResponse>> UpdateProject(long userId, [FromBody] UpdateProjectRequest request)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasProjectAccessAsync(trainerId, userId, request.Id))
                return NotFound();
            try
            {
                var result = await _projectService.UpdateProjectAsync(request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("clients/{userId}/projects/{projectId}")]
        public async Task<ActionResult> DeleteProject(long userId, Guid projectId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasProjectAccessAsync(trainerId, userId, projectId))
                return NotFound();

            await _projectService.DeleteProjectAsync(projectId);

            return Ok();
        }
    }
}
