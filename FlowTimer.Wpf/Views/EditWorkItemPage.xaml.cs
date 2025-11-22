using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class EditWorkItemPage : INavigable
    {
        private readonly EditWorkItemViewModel _viewModel;

        public EditWorkItemPage(EditWorkItemViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is not int workItemId)
            {
                return;
            }

            await _viewModel.Initialize(workItemId);
        }
    }
}