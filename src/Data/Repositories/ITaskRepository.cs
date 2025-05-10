using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ITaskRepository
    {
        Task<NeuroTask?> GetTaskByIdAsync(Guid taskId);
        Task<IList<NeuroTask>> GetTasksByProjectIdAsync(Guid projectId);
        Task<NeuroTask> CreateTaskAsync(NeuroTask task);
        Task<NeuroTask> UpdateTaskAsync(NeuroTask task);
        Task DeleteTaskAsync(Guid taskId);
    }
}
