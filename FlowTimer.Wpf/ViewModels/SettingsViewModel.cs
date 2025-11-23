using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Application.Interfaces;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class SettingsViewModel(ISettingsService settingsService) : ObservableObject
    {
        private readonly ISettingsService _settingsService = settingsService;

        [ObservableProperty]
        private int _selectedThemeIndex;

        public void Initialize()
        {
            var theme = _settingsService.GetTheme();
            SelectedThemeIndex = theme switch
            {
                "Light" => 0,
                "Dark" => 1,
                _ => 2 // System
            };
        }

        private static void ApplyTheme(string theme)
        {
            System.Windows.Application.Current.ThemeMode = theme switch
            {
                "Light" => ThemeMode.Light,
                "Dark" => ThemeMode.Dark,
                _ => ThemeMode.System
            };
        }

        partial void OnSelectedThemeIndexChanged(int value)
        {
            var theme = value switch
            {
                0 => "Light",
                1 => "Dark",
                _ => "System"
            };

            _settingsService.SetTheme(theme);
            ApplyTheme(theme);
        }
    }
}