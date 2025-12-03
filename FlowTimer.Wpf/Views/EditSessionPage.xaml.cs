using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class EditSessionPage : INavigable
    {
        private readonly EditSessionViewModel _viewModel;

        public EditSessionPage(EditSessionViewModel viewModel)
        {
            InitializeComponent();
            
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        public async void OnNavigatedTo(object parameter)
        {
            if (parameter is not int sessionId)
            {
                return;
            }

            await _viewModel.Initialize(sessionId);
        }
    }
}