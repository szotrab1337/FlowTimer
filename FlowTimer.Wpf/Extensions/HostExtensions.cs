using FlowTimer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlowTimer.Wpf.Extensions
{
    public static class HostExtensions
    {
        extension(IHost host)
        {
            public void ApplyMigrations()
            {
                using var scope = host.Services.CreateScope();
                var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FlowTimerDbContext>>();
                using var context = factory.CreateDbContext();
                context.Database.Migrate();
            }
        }
    }
}