using Dapper;
using System.Data;
using System.Text.Json;

namespace Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDbConnection _con;
        private readonly string _taskTable = "tasks";

        public TaskRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<NeuroTask?> GetTaskByIdAsync(Guid taskId)
        {
            var sql = $@"
SELECT
    task_id AS {nameof(NeuroTask.Id)},
    project_id AS {nameof(NeuroTask.ProjectId)},
    created_at AS {nameof(NeuroTask.CreatedAt)},
    section_name AS {nameof(NeuroTask.SectionName)},
    title AS {nameof(NeuroTask.Title)},
    planning_times AS {nameof(NeuroTask.PlanningTimes)},
    fact_times AS {nameof(NeuroTask.FactTimes)}
FROM {_taskTable}
WHERE task_id = @TaskId";

            var task = await _con.QueryFirstOrDefaultAsync<NeuroTask>(sql, new { TaskId = taskId });
            return task;
        }

        public async Task<IList<NeuroTask>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var sql = $@"
SELECT
    task_id AS {nameof(NeuroTask.Id)},
    project_id AS {nameof(NeuroTask.ProjectId)},
    created_at AS {nameof(NeuroTask.CreatedAt)},
    section_name AS {nameof(NeuroTask.SectionName)},
    title AS {nameof(NeuroTask.Title)},
    planning_times AS {nameof(NeuroTask.PlanningTimes)},
    fact_times AS {nameof(NeuroTask.FactTimes)}
FROM {_taskTable}
WHERE project_id = @ProjectId
ORDER BY created_at DESC";

            var tasks = await _con.QueryAsync<NeuroTask>(sql, new { ProjectId = projectId });
            return tasks.ToList();
        }

        public async Task<NeuroTask> CreateTaskAsync(NeuroTask task)
        {
            var sql = $@"
UPSERT INTO {_taskTable}
(
    task_id,
    project_id,
    created_at,
    section_name,
    title,
    planning_times,
    fact_times
)
VALUES
(
    @Id,
    @ProjectId,
    @CreatedAt,
    @SectionName,
    @Title,
    @PlanningTimes,
    @FactTimes
)";
            await _con.ExecuteAsync(sql,
                new
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    CreatedAt = task.CreatedAt,
                    SectionName = task.SectionName.ToString(),
                    Title = task.Title,
                    PlanningTimes = JsonSerializer.Serialize(task.PlanningTimes),
                    FactTimes = JsonSerializer.Serialize(task.FactTimes)
                });
            return task;
        }

        public async Task<NeuroTask> UpdateTaskAsync(NeuroTask task)
        {
            var sql = $@"
UPSERT INTO {_taskTable}
(
    task_id,
    project_id,
    section_name,
    title,
    planning_times,
    fact_times
)
VALUES
(
    @Id,
    @ProjectId,
    @SectionName,
    @Title,
    @PlanningTimes,
    @FactTimes
)";
            await _con.ExecuteAsync(sql,
                new
                {
                    Id = task.Id,
                    ProjectId = task.ProjectId,
                    SectionName = task.SectionName.ToString(),
                    Title = task.Title,
                    PlanningTimes = JsonSerializer.Serialize(task.PlanningTimes),
                    FactTimes = JsonSerializer.Serialize(task.FactTimes)
                });
            return task;
        }

        public async Task DeleteTaskAsync(Guid taskId)
        {
            var sql = $@"
DELETE FROM {_taskTable}
WHERE task_id = @TaskId";
            await _con.ExecuteAsync(sql, new { TaskId = taskId });
        }
    }
}
