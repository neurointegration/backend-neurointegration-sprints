using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskResponse>> GetTaskById(Guid taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IList<TaskResponse>>> GetTasksByProjectId(Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
        {
            var result = await _taskService.CreateTaskAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<TaskResponse>> UpdateTask([FromBody] UpdateTaskRequest request)
        {
            try
            {
                var result = await _taskService.UpdateTaskAsync(request);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
