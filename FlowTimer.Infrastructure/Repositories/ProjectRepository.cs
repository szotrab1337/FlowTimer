using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;
using FlowTimer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowTimer.Infrastructure.Repositories
{
    public class ProjectRepository(IDbContextFactory<FlowTimerDbContext> contextFactory)
        : BaseRepository(contextFactory), IProjectRepository
    {
        public async Task<bool> Add(Project project)
        {
            project.CreatedOn = DateTime.Now;
            project.ModifiedOn = DateTime.Now;

            await using var context = CreateContext();

            context.Projects.Add(project);
            var result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> Archive(int id)
        {
            await using var context = CreateContext();

            var result = await context.Projects
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.IsArchived, true)
                    .SetProperty(z => z.ArchivedOn, DateTime.Now)
                );

            return result > 0;
        }

        public async Task<List<Project>> GetAll()
        {
            await using var context = CreateContext();

            return await context.Projects
                .Include(x => x.WorkItems.Where(y => !y.IsArchived))
                .ThenInclude(z => z.Sessions.Where(y => !y.IsArchived))
                .Where(x => !x.IsArchived)
                .ToListAsync();
        }

        public async Task<Project?> GetById(int id)
        {
            await using var context = CreateContext();

            return await context.Projects
                .Include(x => x.WorkItems.Where(y => !y.IsArchived))
                .ThenInclude(z => z.Sessions.Where(y => !y.IsArchived))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsArchived);
        }

        public async Task<bool> Update(Project project)
        {
            await using var context = CreateContext();

            var result = await context.Projects
                .Where(x => x.Id == project.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.Name, project.Name)
                    .SetProperty(z => z.Description, project.Description)
                    .SetProperty(z => z.ModifiedOn, DateTime.Now)
                );

            return result > 0;
        }
    }
}