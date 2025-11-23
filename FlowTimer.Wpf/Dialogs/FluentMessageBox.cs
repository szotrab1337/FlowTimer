using System.Windows;
using FlowTimer.Wpf.Views.Dialogs;

namespace FlowTimer.Wpf.Dialogs
{
    public static class FluentMessageBox
    {
        public static async Task<bool> Confirm(string message, string title = "Potwierdzenie")
        {
            var dlg = new FluentDialog(title, message, "OK", "Anuluj");
            var owner = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (owner is null)
            {
                return false;
            }

            var result = await dlg.ShowDialogAsync(owner);

            return result == FluentDialog.DialogResultType.Primary;
        }

        public static async Task ShowInfo(string message, string title = "Informacja")
        {
            var dlg = new FluentDialog(title, message, "OK", null);
            var owner = System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (owner is null)
            {
                return;
            }

            await dlg.ShowDialogAsync(owner);
        }
    }
}