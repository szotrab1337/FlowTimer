using FlowTimer.Application.Interfaces;
using FlowTimer.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlowTimer.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public void AddApplication()
            {
                services.AddScoped<IProjectService, ProjectService>();
                services.AddScoped<IWorkItemService, WorkItemService>();
            }
        }
    }
}