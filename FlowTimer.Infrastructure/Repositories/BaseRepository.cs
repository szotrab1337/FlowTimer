using FlowTimer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowTimer.Infrastructure.Repositories
{
    public class BaseRepository(IDbContextFactory<FlowTimerDbContext> contextFactory)
    {
        private readonly IDbContextFactory<FlowTimerDbContext> _contextFactory = contextFactory;

        protected FlowTimerDbContext CreateContext()
        {
            return _contextFactory.CreateDbContext();
        }
    }
}