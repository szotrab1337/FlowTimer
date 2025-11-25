using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class CompactTimerViewModel(
        ISessionTimerService sessionTimerService) : ObservableObject
    {
        private readonly ISessionTimerService _sessionTimerService = sessionTimerService;

        [ObservableProperty]
        private string _activeSessionTime = "00:00:00";

        public void Cleanup()
        {
            _sessionTimerService.Tick -= OnTimerTick;
        }

        public void Initialize()
        {
            _sessionTimerService.Tick += OnTimerTick;
        }

        private void OnTimerTick(object? sender, SessionTimerTickEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                ActiveSessionTime = $"{(int)e.Elapsed.TotalHours:00}:{e.Elapsed.Minutes:00}:{e.Elapsed.Seconds:00}";
            });
        }

        [RelayCommand]
        private async Task StopTimer()
        {
            await _sessionTimerService.Stop();
        }
    }
}