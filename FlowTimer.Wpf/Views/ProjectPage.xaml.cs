using System.Windows.Controls;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class ProjectPage : Page, INavigable
    {
        private readonly ProjectViewModel _viewModel;

        public ProjectPage(ProjectViewModel viewModel)
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