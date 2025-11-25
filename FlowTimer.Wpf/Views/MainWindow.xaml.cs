using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MenuItem = FlowTimer.Wpf.Models.MenuItem;

namespace FlowTimer.Wpf.Views
{
    public partial class MainWindow
    {
        private readonly IHost _host;
        private readonly MainViewModel _viewModel;

        public MainWindow(
            MainViewModel viewModel,
            INavigationService navigationService,
            IHost host)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _host = host;
            DataContext = _viewModel;

            navigationService.SetFrame(RootContentFrame);

            _viewModel.Initialize();
        }

        protected override Grid BaseMainGrid => MainGrid;
        protected override Border BaseHighContrastBorder => HighContrastBorder;
        protected override Button BaseCloseButton => CloseButton;
        protected override Button? BaseMinimizeButton => MinimizeButton;
        protected override Button? BaseMaximizeButton => MaximizeButton;

        private void Menu_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Menu.SelectedItem is not MenuItem navItem)
            {
                return;
            }

            _viewModel.Navigate(navItem);
        }

        private void OnSwitchToCompactWindow_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            var compactTimerWindow = _host.Services.GetRequiredService<CompactTimerWindow>();

            compactTimerWindow.Show();
            compactTimerWindow.Closed += (_, _) => Show();
        }
    }
}