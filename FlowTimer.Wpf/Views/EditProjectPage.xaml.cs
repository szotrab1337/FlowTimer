using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class EditProjectPage : INavigable
    {
        private readonly EditProjectViewModel _viewModel;

        public EditProjectPage(EditProjectViewModel viewModel)
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
    }
}