using Service.Dto;

namespace Service
{
    public interface IProjectService
    {
        Task<ProjectResponse?> GetProjectByIdAsync(Guid projectId);
        Task<IList<ProjectResponse>> GetProjectsBySprintAsync(long userId, long sprintNumber);
        Task<ProjectResponse> CreateProjectAsync(long userId, CreateProjectRequest request);
        Task<ProjectResponse> UpdateProjectAsync(UpdateProjectRequest request);
        Task DeleteProjectAsync(Guid projectId);
    }
}
