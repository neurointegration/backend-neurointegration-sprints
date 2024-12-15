using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;

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

        [HttpGet("sprint/{sprintId}")]
        public async Task<ActionResult<IList<ProjectResponse>>> GetProjectsBySprintId(Guid sprintId)
        {
            var projects = await _projectService.GetProjectsBySprintIdAsync(sprintId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> CreateProject([FromBody] CreateProjectRequest request)
        {
            var result = await _projectService.CreateProjectAsync(request);
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
    }
}
