using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetProjectByIdAsync(Guid projectId);
        Task<IList<Project>> GetProjectsBySprintIdAsync(Guid sprintId);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
    }
}
