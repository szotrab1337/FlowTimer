using System.Windows;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class ProjectsPage
    {
        private readonly ProjectsViewModel _viewModel;

        public ProjectsPage(ProjectsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private async void Page_OnLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.Initialize();
        }
    }
}