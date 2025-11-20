using System.Windows;
using FlowTimer.Wpf.Extensions;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlowTimer.Wpf
{
    public partial class App
    {
        private static readonly IHost Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices((_, services) =>
            {
                services.AddWpf();
            }).Build();

        [STAThread]
        public static void Main()
        {
            Host.Start();
            
            var app = new App();
            app.InitializeComponent();
            app.MainWindow = Host.Services.GetRequiredService<MainWindow>();
            app.MainWindow.Visibility = Visibility.Visible;
            app.Run();
        }
    }
}