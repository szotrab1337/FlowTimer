using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlowTimer.Infrastructure.Persistence
{
    public class FlowTimerDbContextFactory : IDesignTimeDbContextFactory<FlowTimerDbContext>
    {
        public FlowTimerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FlowTimerDbContext>();
            optionsBuilder.UseSqlite("Data Source=flowtimer.db");

            return new FlowTimerDbContext(optionsBuilder.Options);
        }
    }
}