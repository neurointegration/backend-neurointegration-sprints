using Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISprintService
    {
        Task<SprintResponse> GetSprintByIdAsync(Guid userId, Guid sprintId);
        Task<IList<SprintResponse>> GetSprintsByUserIdAsync(Guid userId);
        Task CreateSprintAsync(Guid userId, CreateSprintRequest request);
    }
}
