using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.Views;
using MenuItem = FlowTimer.Wpf.Models.MenuItem;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class MainViewModel(
        INavigationService navigationService,
        ISessionTimerService sessionTimerService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISessionTimerService _sessionTimerService = sessionTimerService;

        [ObservableProperty]
        private string _activeSessionTime = "00:00:00";

        [ObservableProperty]
        private bool _canNavigateBack;

        [ObservableProperty]
        private bool _isTimerRunning;

        [ObservableProperty]
        private ICollection<MenuItem> _menuItems =
        [
            new()
            {
                Title = "Strona główna",
                TargetPage = typeof(HomePage),
                Icon = "\ue80f"
            },
            new()
            {
                Title = "Projekty",
                TargetPage = typeof(ProjectsPage),
                Icon = "\uebc6"
            }
        ];

        public void Initialize()
        {
            IsTimerRunning = _sessionTimerService.IsRunning;

            _sessionTimerService.Tick += OnTimerTick;
            _sessionTimerService.SessionStarted += OnSessionStarted;
            _sessionTimerService.SessionStopped += OnSessionStopped;
        }

        public void Navigate(MenuItem? value)
        {
            if (value is null)
            {
                return;
            }

            _navigationService.Navigate(value.TargetPage);
        }

        [RelayCommand]
        private void GoToActiveProject()
        {
            if (_sessionTimerService.ActiveProjectId.HasValue)
            {
                _navigationService.Navigate(typeof(ProjectDashboardPage), _sessionTimerService.ActiveProjectId.Value);
            }
        }

        private void OnSessionStarted(object? sender, SessionStartedEventArgs e)
        {
            IsTimerRunning = true;
        }

        private void OnSessionStopped(object? sender, SessionStoppedEventArgs e)
        {
            IsTimerRunning = false;
            ActiveSessionTime = "00:00:00";
        }

        private void OnTimerTick(object? sender, SessionTimerTickEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                ActiveSessionTime = $"{(int)e.Elapsed.TotalHours:00}:{e.Elapsed.Minutes:00}:{e.Elapsed.Seconds:00}";
            });
        }

        [RelayCommand]
        private void Settings()
        {
            _navigationService.Navigate(typeof(SettingsPage));
        }

        [RelayCommand]
        private async Task StopTimer()
        {
            await _sessionTimerService.Stop();
        }
    }
}