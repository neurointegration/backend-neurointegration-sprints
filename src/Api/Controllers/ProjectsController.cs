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
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectResponse>> GetProjectById(Guid projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpGet("sprint/{sprintNumber}")]
        public async Task<ActionResult<IList<ProjectResponse>>> GetProjectsBySprintId(long sprintNumber)
        {
            var userId = User.GetUserId();
            var projects = await _projectService.GetProjectsBySprintAsync(userId, sprintNumber);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> CreateProject([FromBody] CreateProjectRequest request)
        {
            var userId = User.GetUserId();
            var result = await _projectService.CreateProjectAsync(userId, request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ProjectResponse>> UpdateProject([FromBody] UpdateProjectRequest request)
        {
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

        [HttpDelete("{projectId}")]
        public async Task<ActionResult> DeleteProject(Guid projectId)
        {
            await _projectService.DeleteProjectAsync(projectId);
            return Ok();
        }
    }
}
