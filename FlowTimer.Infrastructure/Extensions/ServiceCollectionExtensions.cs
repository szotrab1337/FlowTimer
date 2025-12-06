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
                var connectionStringTemplate = configuration.GetConnectionString("FlowTimerDb");
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var connectionString = connectionStringTemplate!.Replace("{AppData}", appData);

                services.AddDbContextFactory<FlowTimerDbContext>(
                    options => { options.UseSqlite(connectionString); },
                    ServiceLifetime.Transient);

                services.AddScoped<IProjectRepository, ProjectRepository>();
                services.AddScoped<IWorkItemRepository, WorkItemRepository>();
                services.AddScoped<ISessionRepository, SessionRepository>();
                services.AddSingleton<ISettingsService, SettingsService>();
            }
        }
    }
}