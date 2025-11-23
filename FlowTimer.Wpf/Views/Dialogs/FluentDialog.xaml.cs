using System.Windows;

namespace FlowTimer.Wpf.Views.Dialogs
{
    public partial class FluentDialog
    {
        public enum DialogResultType
        {
            Primary,
            Secondary,
            None
        }

        private readonly TaskCompletionSource<DialogResultType> _tcs = new();

        public FluentDialog(string title, string message, string primary, string? secondary)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;

            PrimaryButton.Content = primary;

            if (secondary is null)
            {
                SecondaryButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                SecondaryButton.Content = secondary;
            }
            
            KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    SecondaryButton_Click(s, new RoutedEventArgs());
                }
                else if (e.Key == System.Windows.Input.Key.Enter)
                {
                    PrimaryButton_Click(s, new RoutedEventArgs());
                }
            };
        }

        public Task<DialogResultType> ShowDialogAsync(Window owner)
        {
            Owner = owner;
            ShowDialog();

            return _tcs.Task;
        }

        private void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            _tcs.TrySetResult(DialogResultType.Primary);
            Close();
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            _tcs.TrySetResult(DialogResultType.Secondary);
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _tcs.TrySetResult(DialogResultType.None);
            base.OnClosed(e);
        }
    }
}