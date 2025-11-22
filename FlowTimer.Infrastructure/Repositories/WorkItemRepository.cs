using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;
using FlowTimer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowTimer.Infrastructure.Repositories
{
    public class WorkItemRepository(IDbContextFactory<FlowTimerDbContext> contextFactory)
        : BaseRepository(contextFactory), IWorkItemRepository
    {
        public async Task<bool> Add(WorkItem workItem)
        {
            workItem.CreatedOn = DateTime.Now;
            workItem.ModifiedOn = DateTime.Now;

            await using var context = CreateContext();

            context.WorkItems.Add(workItem);
            var result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> Archive(int id)
        {
            await using var context = CreateContext();

            var result = await context.WorkItems
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.IsArchived, true)
                    .SetProperty(z => z.ArchivedOn, DateTime.Now)
                );

            return result > 0;
        }

        public async Task<WorkItem?> GetById(int id)
        {
            await using var context = CreateContext();

            return await context.WorkItems
                .Include(x => x.Sessions.Where(y => !y.IsArchived))
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsArchived);
        }

        public async Task<List<WorkItem>> GetByProjectId(int projectId)
        {
            await using var context = CreateContext();

            return await context.WorkItems
                .Include(x => x.Sessions.Where(y => !y.IsArchived))
                .Where(x => x.ProjectId == projectId && !x.IsArchived)
                .OrderBy(x => x.IsCompleted)
                .ThenBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<bool> Update(WorkItem workItem)
        {
            await using var context = CreateContext();

            var result = await context.WorkItems
                .Where(x => x.Id == workItem.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.Name, workItem.Name)
                    .SetProperty(z => z.Description, workItem.Description)
                    .SetProperty(z => z.IsCompleted, workItem.IsCompleted)
                    .SetProperty(z => z.ModifiedOn, DateTime.Now)
                );

            return result > 0;
        }
    }
}