using System.Windows;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class HomePage
    {
        private readonly HomeViewModel _viewModel;

        public HomePage(HomeViewModel viewModel)
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