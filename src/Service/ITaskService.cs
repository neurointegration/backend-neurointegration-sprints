using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITaskService
    {
        Task<TaskResponse?> GetTaskByIdAsync(Guid taskId);
        Task<IList<TaskResponse>> GetTasksByProjectIdAsync(Guid projectId);
        Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request);
        Task<TaskResponse> UpdateTaskAsync(UpdateTaskRequest request);
    }
}
