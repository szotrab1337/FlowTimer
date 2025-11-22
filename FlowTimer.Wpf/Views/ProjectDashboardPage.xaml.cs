using System.Windows;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class ProjectDashboardPage : INavigable
    {
        private readonly ProjectDashboardViewModel _viewModel;

        public ProjectDashboardPage(ProjectDashboardViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is not int projectId)
            {
                return;
            }

            await _viewModel.Initialize(projectId);
        }

        private void Page_OnUnloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Cleanup();
        }
    }
}