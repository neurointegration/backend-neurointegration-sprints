using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/Trainer")]
    [Authorize(Roles = "Trainer")]
    public class TrainerTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TrainerTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("clients/{userId}/tasks/{taskId}")]
        public async Task<ActionResult<TaskResponse>> GetTaskById(Guid userId, Guid taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("clients/{userId}/tasks/project/{projectId}")]
        public async Task<ActionResult<IList<TaskResponse>>> GetTasksByProjectId(Guid userId, Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }
    }
}
