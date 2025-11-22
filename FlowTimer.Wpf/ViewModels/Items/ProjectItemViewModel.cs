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

        public ProjectItemViewModel(Project project)
        {
            LoadValues(project);
        }

        public void AddSessionTime(TimeSpan duration)
        {
            _baseTicks += duration.Ticks;
            UpdateTime(TimeSpan.Zero);
        }

        public void Update(Project project)
        {
            LoadValues(project);
        }

        public void UpdateTime(TimeSpan elapsed)
        {
            var total = TimeSpan.FromTicks(_baseTicks) + elapsed;
            TotalTime = $"{(int)total.TotalHours:00}:{total.Minutes:00}:{total.Seconds:00}";
        }

        private void LoadValues(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            CreatedOn = project.CreatedOn;
            ModifiedOn = project.ModifiedOn;

            var totalTasks = project.WorkItems.Count;
            var completedTasks = project.WorkItems.Count(x => x.IsCompleted);
            TasksProgress = $"Ukończono {completedTasks} z {totalTasks} zadań";

            var duration = TimeSpan.FromTicks(project.WorkItems.Sum(x => x.Duration.Ticks));
            TotalTime = $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
            
            _baseTicks = duration.Ticks;
        }
    }
}