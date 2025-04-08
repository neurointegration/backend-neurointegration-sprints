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
        Task<SprintResponse> GetSprintByIdAsync(long userId, long sprintNumber);
        Task<IList<SprintResponse>> GetSprintsByUserIdAsync(long userId);
        Task CreateSprintAsync(long userId, CreateSprintRequest request);
    }
}
