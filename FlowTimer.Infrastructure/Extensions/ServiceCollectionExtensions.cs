using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Repositories;
using FlowTimer.Infrastructure.Persistence;
using FlowTimer.Infrastructure.Repositories;
using FlowTimer.Infrastructure.Settings;
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
                services.AddDbContextFactory<FlowTimerDbContext>(
                    options => { options.UseSqlite(configuration.GetConnectionString("FlowTimerDb")); },
                    ServiceLifetime.Transient);

                services.AddScoped<IProjectRepository, ProjectRepository>();
                services.AddScoped<IWorkItemRepository, WorkItemRepository>();
                services.AddScoped<ISessionRepository, SessionRepository>();
                services.AddSingleton<ISettingsService, SettingsService>();
            }
        }
    }
}