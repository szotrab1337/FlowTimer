using FlowTimer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowTimer.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public void AddInfrastructure(IConfiguration configuration)
            {
                services.AddDbContext<FlowTimerDbContext>(
                    options => { options.UseSqlite(configuration.GetConnectionString("FlowTimerDb")); },
                    ServiceLifetime.Transient);
            }
        }
    }
}