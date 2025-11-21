using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Domain.Entities;

namespace FlowTimer.Wpf.ViewModels.Items
{
    public partial class ProjectViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _createdOn;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private bool _isArchived;

        [ObservableProperty]
        private DateTime _modifiedOn;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _tasksProgress;

        [ObservableProperty]
        private string _totalTime;

        public ProjectViewModel(Project project)
        {
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            IsArchived = project.IsArchived;
            CreatedOn = project.CreatedOn;
            ModifiedOn = project.ModifiedOn;

            var totalTasks = project.WorkItems.Count;
            var completedTasks = project.WorkItems.Count(x => x.IsCompleted);
            TasksProgress = $"Ukończono {completedTasks} z {totalTasks} zadań";

            var duration = TimeSpan.FromTicks(project.WorkItems.Sum(x => x.Duration.Ticks));
            TotalTime = $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
        }
    }
}