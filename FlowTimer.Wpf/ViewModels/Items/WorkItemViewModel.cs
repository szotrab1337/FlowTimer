using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Domain.Entities;

namespace FlowTimer.Wpf.ViewModels.Items
{
    public partial class WorkItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _createdOn;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private bool _isCompleted;

        [ObservableProperty]
        private DateTime _modifiedOn;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _projectId;

        [ObservableProperty]
        private string _totalTime = string.Empty;

        [ObservableProperty]
        private bool _isSessionActive;

        public WorkItemViewModel(WorkItem workItem)
        {
            LoadValues(workItem);
        }

        private void LoadValues(WorkItem workItem)
        {
            Id = workItem.Id;
            ProjectId = workItem.ProjectId;
            Name = workItem.Name;
            Description = workItem.Description;
            IsCompleted = workItem.IsCompleted;
            CreatedOn = workItem.CreatedOn;
            ModifiedOn = workItem.ModifiedOn;

            var duration = TimeSpan.FromTicks(workItem.Sessions.Sum(x => x.Duration.Ticks));
            TotalTime = $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
        }
    }
}