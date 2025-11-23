using System.Windows;
using FlowTimer.Application.Extensions;
using FlowTimer.Application.Interfaces;
using FlowTimer.Infrastructure.Extensions;
using FlowTimer.Wpf.Extensions;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlowTimer.Wpf
{
    public partial class App
    {
        private static readonly IHost Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureLogging((context, builder) => { builder.AddLogger(context.Configuration); })
            .ConfigureServices((context, services) =>
            {
                services.AddWpf();
                services.AddApplication();
                services.AddInfrastructure(context.Configuration);
            }).Build();

        [STAThread]
        public static void Main()
        {
            Host.ApplyMigrations();
            Host.Start();

            var app = new App();
            app.InitializeComponent();
            app.MainWindow = Host.Services.GetRequiredService<MainWindow>();
            app.MainWindow.Visibility = Visibility.Visible;
            app.Run();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            var timerService = Host.Services.GetRequiredService<ISessionTimerService>();
            if (timerService.IsRunning)
            {
                await timerService.Stop();
            }

            base.OnExit(e);
        }
    }
}