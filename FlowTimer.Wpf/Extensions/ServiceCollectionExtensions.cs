using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;

namespace FlowTimer.Wpf.Extensions
{
    public static class ServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public void AddWpf()
            {
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainViewModel>();

                services.AddTransient<HomePage>();
                services.AddTransient<SettingsPage>();

                services.AddTransient<ProjectsPage>();
                services.AddTransient<ProjectsViewModel>();

                services.AddTransient<ProjectDashboardPage>();
                services.AddTransient<ProjectDashboardViewModel>();

                services.AddTransient<AddProjectPage>();
                services.AddTransient<AddProjectViewModel>();
            }
        }
    }
}