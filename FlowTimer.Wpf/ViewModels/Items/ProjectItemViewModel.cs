using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Domain.Entities;

namespace FlowTimer.Wpf.ViewModels.Items
{
    public partial class ProjectItemViewModel : ObservableObject
    {
        private long _baseTicks;

        [ObservableProperty]
        private DateTime _createdOn;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private bool _isSessionActive;

        [ObservableProperty]
        private DateTime _modifiedOn;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _tasksProgress = string.Empty;

        [ObservableProperty]
        private string _totalTime = string.Empty;

        public ProjectItemViewModel(Project project, int? activeSessionId = null)
        {
            LoadValues(project, activeSessionId);
        }

        public void UpdateTime(TimeSpan elapsed)
        {
            var total = TimeSpan.FromTicks(_baseTicks) + elapsed;
            TotalTime = $"{(int)total.TotalHours:00}:{total.Minutes:00}:{total.Seconds:00}";
        }

        private void LoadValues(Project project, int? activeSessionId = null)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            CreatedOn = project.CreatedOn;
            ModifiedOn = project.ModifiedOn;

            var totalTasks = project.WorkItems.Count;
            var completedTasks = project.WorkItems.Count(x => x.IsCompleted);
            TasksProgress = $"Ukończono {completedTasks} z {totalTasks} zadań";

            _baseTicks = project.WorkItems
                .Sum(x => x.Sessions
                    .Where(z => z.Id != activeSessionId)
                    .Sum(z => z.Duration.Ticks));

            UpdateTime(TimeSpan.Zero);
        }
    }
}