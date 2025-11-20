namespace FlowTimer.Wpf.Helpers
{
    internal static class Utility
    {
        public static bool IsBackdropDisabled()
        {
            var appContextBackdropData =
                AppContext.GetData("Switch.System.Windows.Appearance.DisableFluentThemeWindowBackdrop");
            var disableFluentThemeWindowBackdrop = false;

            if (appContextBackdropData != null)
            {
                disableFluentThemeWindowBackdrop = bool.Parse(Convert.ToString(appContextBackdropData) ?? string.Empty);
            }

            return disableFluentThemeWindowBackdrop;
        }

        public static bool IsBackdropSupported()
        {
            var os = Environment.OSVersion;
            var version = os.Version;

            return version is { Major: >= 10, Build: >= 22621 };
        }
    }
}