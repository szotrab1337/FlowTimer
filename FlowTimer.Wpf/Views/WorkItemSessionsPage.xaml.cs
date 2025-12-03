using System.Windows;
using FlowTimer.Domain.Entities;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class WorkItemSessionsPage : INavigable
    {
        private readonly WorkItemSessionsViewModel _viewModel;

        public WorkItemSessionsPage(WorkItemSessionsViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is not WorkItem workItem)
            {
                return;
            }

            await _viewModel.Initialize(workItem);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Cleanup();
        }
    }
}