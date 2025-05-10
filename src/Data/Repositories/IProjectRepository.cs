namespace Data.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetProjectByIdAsync(Guid projectId);
        Task<IList<Project>> GetProjectsBySprintAsync(long userId, long sprintNumber);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Guid projectId);
    }
}
