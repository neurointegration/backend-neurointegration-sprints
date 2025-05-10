using Dapper;
using System.Data;
using System.Text.Json;

namespace Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IDbConnection _con;
        private readonly string _projectsTable = "projects              ";
        private readonly string _tasksTable = "tasks";

        public ProjectRepository(IDbConnection con)
        {
            _con = con;
        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId)
        {
            var sql = $@"
SELECT 
    project_id AS {nameof(Project.Id)},
    chat_id AS {nameof(Project.UserId)},
    sprint_number AS {nameof(Project.SprintNumber)},
    section_name AS {nameof(Project.SectionName)},
    title AS {nameof(Project.Title)},
    planning_times AS {nameof(Project.PlanningTimes)},
    fact_times AS {nameof(Project.FactTimes)}
FROM {_projectsTable}
WHERE project_id = @ProjectId";

            return await _con.QueryFirstOrDefaultAsync<Project>(sql, new { ProjectId = projectId });
        }

        public async Task<IList<Project>> GetProjectsBySprintAsync(long userId, long sprintNumber)
        {
            var sql = $@"
SELECT 
    project_id AS {nameof(Project.Id)},
    chat_id AS {nameof(Project.UserId)},
    sprint_number AS {nameof(Project.SprintNumber)},
    section_name AS {nameof(Project.SectionName)},
    title AS {nameof(Project.Title)},
    planning_times AS {nameof(Project.PlanningTimes)},
    fact_times AS {nameof(Project.FactTimes)}
FROM {_projectsTable}
WHERE chat_id = @UserId AND sprint_number = @SprintNumber";

            var projects = await _con.QueryAsync<Project>(sql, new { UserId = userId, SprintNumber = sprintNumber });
            return projects.ToList();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            var sql = $@"
UPSERT INTO {_projectsTable}
(
    project_id,
    chat_id,
    sprint_number,
    section_name,
    title,
    planning_times,
    fact_times
)
VALUES
(
    @Id,
    @UserId,
    @SprintNumber,
    @SectionName,
    @Title,
    @PlanningTimes,
    @FactTimes
)";

            await _con.ExecuteAsync(sql,
                new
                {
                    Id = project.Id,
                    UserId = project.UserId,
                    SprintNumber = project.SprintNumber,
                    SectionName = project.SectionName.ToString(),
                    Title = project.Title,
                    PlanningTimes = JsonSerializer.Serialize(project.PlanningTimes),
                    FactTimes = JsonSerializer.Serialize(project.FactTimes)
                });
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            var sql = $@"
UPSERT INTO {_projectsTable}
(
    project_id,
    chat_id,
    sprint_number,
    section_name,
    title,
    planning_times,
    fact_times
)
VALUES
(
    @Id,
    @UserId,
    @SprintNumber,
    @SectionName,
    @Title,
    @PlanningTimes,
    @FactTimes
)";
            await _con.ExecuteAsync(sql,
                new
                {
                    Id = project.Id,
                    UserId = project.UserId,
                    SprintNumber = project.SprintNumber,
                    SectionName = project.SectionName.ToString(),
                    Title = project.Title,
                    PlanningTimes = JsonSerializer.Serialize(project.PlanningTimes),
                    FactTimes = JsonSerializer.Serialize(project.FactTimes)
                });
            return project;
        }

        public async Task DeleteProjectAsync(Guid projectId)
        {
            var deleteTasksSql = $@"
DELETE FROM {_tasksTable}
WHERE project_id = @ProjectId";
            await _con.ExecuteAsync(deleteTasksSql,
                new { ProjectId = projectId });

            var deleteProjectSql = $@"
DELETE FROM {_projectsTable}
WHERE project_id = @ProjectId";
            await _con.ExecuteAsync(deleteProjectSql,
                new { ProjectId = projectId });
        }
    }
}
