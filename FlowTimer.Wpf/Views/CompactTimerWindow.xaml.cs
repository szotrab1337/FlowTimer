using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FlowTimer.Wpf.ViewModels;

namespace FlowTimer.Wpf.Views
{
    public partial class CompactTimerWindow
    {
        private readonly CompactTimerViewModel _viewModel;

        public CompactTimerWindow(CompactTimerViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        protected override Grid BaseMainGrid => MainGrid;
        protected override Border BaseHighContrastBorder => HighContrastBorder;
        protected override Button BaseCloseButton => CloseButton;
        protected override Button? BaseMinimizeButton => null;
        protected override Button? BaseMaximizeButton => null;

        private void StopTimer_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Deactivated(object? sender, EventArgs e)
        {
            Topmost = false;
            Topmost = true;
        }

        private void Window_OnClosing(object? sender, CancelEventArgs e)
        {
            _viewModel.Cleanup();
        }

        private void Window_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize();
        }
    }
}