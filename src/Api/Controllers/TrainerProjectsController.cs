using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/Trainer")]
    [Authorize(Roles = "Trainer")]
    public class TrainerProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public TrainerProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("clients/{userId}/projects/{projectId}")]
        public async Task<ActionResult<ProjectResponse>> GetProjectById(long userId, Guid projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("clients/{userId}/projects/sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<ProjectResponse>>> GetProjectsBySprintId(long userId, long sprintNumber)
        {
            var projects = await _projectService.GetProjectsBySprintAsync(userId, sprintNumber);
            return Ok(projects);
        }
    }
}
