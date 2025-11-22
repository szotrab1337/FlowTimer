using System.Windows.Controls;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class ProjectDashboardPage : Page, INavigable
    {
        private readonly ProjectDashboardViewModel _viewModel;

        public ProjectDashboardPage(ProjectDashboardViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is not int projectId)
            {
                return;
            }
            
            _viewModel.Initialize(projectId);
        }
    }
}