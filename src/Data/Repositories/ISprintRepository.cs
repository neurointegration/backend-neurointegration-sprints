using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ISprintRepository
    {
        Task<Sprint?> GetSprintByIdAsync(Guid id);
        Task CreateSprintAsync(Sprint sprint);
        Task UpdateSprintAsync(Sprint sprint);
        Task<IList<Sprint>> GetSprintsByUserIdAsync(Guid userId);
    }
}
