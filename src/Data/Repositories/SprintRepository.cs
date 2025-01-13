using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SprintRepository : ISprintRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SprintRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sprint?> GetSprintByIdAsync(Guid id)
        {
            return await _dbContext.Sprints
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateSprintAsync(Sprint sprint)
        {
            await _dbContext.Sprints.AddAsync(sprint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSprintAsync(Sprint sprint)
        {
            _dbContext.Sprints.Update(sprint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IList<Sprint>> GetSprintsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Sprints
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.BeginDate)
                .ToListAsync();
        }
    }
}
