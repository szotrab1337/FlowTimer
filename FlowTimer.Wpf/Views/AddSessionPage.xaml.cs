using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class AddSessionPage : INavigable
    {
        private readonly AddSessionViewModel _viewModel;

        public AddSessionPage(AddSessionViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is not int workItemId)
            {
                return;
            }

            _viewModel.Initialize(workItemId);
        }
    }
}