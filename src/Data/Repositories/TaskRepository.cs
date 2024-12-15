using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NeuroTask?> GetTaskByIdAsync(Guid taskId)
        {
            return await _context.NeuroTasks
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<IList<NeuroTask>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _context.NeuroTasks
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<NeuroTask> CreateTaskAsync(NeuroTask task)
        {
            await _context.NeuroTasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<NeuroTask> UpdateTaskAsync(NeuroTask task)
        {
            _context.NeuroTasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}
