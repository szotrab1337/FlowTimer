using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;
using FlowTimer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowTimer.Infrastructure.Repositories
{
    public class SessionRepository(IDbContextFactory<FlowTimerDbContext> contextFactory)
        : BaseRepository(contextFactory), ISessionRepository
    {
        public async Task<bool> Add(Session session)
        {
            session.CreatedOn = DateTime.Now;
            session.ModifiedOn = DateTime.Now;

            await using var context = CreateContext();

            context.Sessions.Add(session);
            var result = await context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> Archive(int id)
        {
            await using var context = CreateContext();

            var result = await context.Sessions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.IsArchived, true)
                    .SetProperty(z => z.ArchivedOn, DateTime.Now)
                );

            return result > 0;
        }

        public async Task<Session?> GetById(int id)
        {
            await using var context = CreateContext();

            return await context.Sessions
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsArchived);
        }

        public async Task<List<Session>> GetByWorkItemId(int workItemId)
        {
            await using var context = CreateContext();

            return await context.Sessions
                .Where(x => x.WorkItemId == workItemId && !x.IsArchived)
                .ToListAsync();
        }

        public async Task<bool> Update(Session session)
        {
            await using var context = CreateContext();

            var result = await context.Sessions
                .Where(x => x.Id == session.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.EndTime, session.EndTime)
                    .SetProperty(z => z.IsManual, session.IsManual)
                    .SetProperty(z => z.ModifiedOn, DateTime.Now)
                );

            return result > 0;
        }

        public async Task<bool> UpdateEndTime(int id, DateTime endTime)
        {
            await using var context = CreateContext();

            var result = await context.Sessions
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.EndTime, endTime)
                    .SetProperty(z => z.ModifiedOn, DateTime.Now)
                );

            return result > 0;
        }
    }
}