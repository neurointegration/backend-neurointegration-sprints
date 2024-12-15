using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<IList<Project>> GetProjectsBySprintIdAsync(Guid sprintId)
        {
            return await _context.Projects
                .Where(p => p.SprintId == sprintId)
                .ToListAsync();
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }
    }
}
