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
    public class TrainerTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ITrainerService _trainerService;

        public TrainerTasksController(ITaskService taskService, ITrainerService trainerService)
        {
            _taskService = taskService;
            _trainerService = trainerService;
        }

        [HttpGet("clients/{userId}/tasks/{taskId}")]
        public async Task<ActionResult<TaskResponse>> GetTaskById(long userId, Guid taskId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasTaskAccessAsync(trainerId, userId, taskId))
                return NotFound();

            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("clients/{userId}/tasks/project/{projectId}")]
        public async Task<ActionResult<IList<TaskResponse>>> GetTasksByProjectId(long userId, Guid projectId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasProjectAccessAsync(trainerId, userId, projectId))
                return NotFound();

            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost("clients/{userId}/tasks")]
        public async Task<ActionResult<TaskResponse>> CreateTask(long userId, [FromBody] CreateTaskRequest request)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasProjectAccessAsync(trainerId, userId, request.ProjectId))
                return NotFound();

            var result = await _taskService.CreateTaskAsync(request);
            return Ok(result);
        }

        [HttpPut("clients/{userId}/tasks")]
        public async Task<ActionResult<TaskResponse>> UpdateTask(long userId, [FromBody] UpdateTaskRequest request)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasTaskAccessAsync(trainerId, userId, request.Id))
                return NotFound();

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

        [HttpDelete("clients/{userId}/tasks/{taskId}")]
        public async Task<ActionResult> DeleteTask(long userId, Guid taskId)
        {
            var trainerId = User.GetUserId();
            if (!await _trainerService.HasTaskAccessAsync(trainerId, userId, taskId))
                return NotFound();

            await _taskService.DeleteTaskAsync(taskId);

            return Ok();
        }
    }
}
