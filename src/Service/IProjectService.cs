using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProjectService
    {
        Task<ProjectResponse?> GetProjectByIdAsync(Guid projectId);
        Task<IList<ProjectResponse>> GetProjectsBySprintIdAsync(Guid sprintId);
        Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request);
        Task<ProjectResponse> UpdateProjectAsync(UpdateProjectRequest request);
    }
}
