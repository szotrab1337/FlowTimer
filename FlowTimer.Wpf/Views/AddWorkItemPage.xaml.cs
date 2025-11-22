using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class AddWorkItemPage : INavigable
    {
        private readonly AddWorkItemViewModel _viewModel;

        public AddWorkItemPage(AddWorkItemViewModel viewModel)
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