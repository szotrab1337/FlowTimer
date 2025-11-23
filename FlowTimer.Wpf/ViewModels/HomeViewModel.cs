using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Application.Interfaces;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class HomeViewModel(
        IDashboardService dashboardService) : ObservableObject
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [ObservableProperty]
        private int _pendingTasksCount;

        [ObservableProperty]
        private int _projectsCount;

        [ObservableProperty]
        private int _sessionsCount;

        [ObservableProperty]
        private string _totalTimeFormatted = "00:00:00";

        public async Task Initialize()
        {
            var stats = await _dashboardService.GetStatistics();

            ProjectsCount = stats.ProjectsCount;
            PendingTasksCount = stats.PendingTasksCount;
            SessionsCount = stats.SessionsCount;

            TotalTimeFormatted =
                $"{(int)stats.TotalTime.TotalHours:00}:{stats.TotalTime.Minutes:00}:{stats.TotalTime.Seconds:00}";
        }
    }
}