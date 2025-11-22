using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class AddProjectPage
    {
        public AddProjectPage(AddProjectViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}