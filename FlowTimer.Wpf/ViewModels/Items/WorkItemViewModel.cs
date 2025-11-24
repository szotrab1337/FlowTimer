using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Domain.Entities;

namespace FlowTimer.Wpf.ViewModels.Items
{
    public partial class WorkItemViewModel : ObservableObject
    {
        private long _baseTicks;

        [ObservableProperty]
        private DateTime _createdOn;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private bool _isCompleted;

        [ObservableProperty]
        private bool _isSessionActive;

        [ObservableProperty]
        private DateTime _modifiedOn;

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private int _projectId;

        [ObservableProperty]
        private string _totalTime = string.Empty;

        public WorkItemViewModel(WorkItem workItem, int? activeSessionId = null)
        {
            LoadValues(workItem, activeSessionId);
        }

        public void AddSessionTime(TimeSpan duration)
        {
            _baseTicks += duration.Ticks;
            UpdateTime(TimeSpan.Zero);
        }

        public void UpdateTime(TimeSpan elapsed)
        {
            var total = TimeSpan.FromTicks(_baseTicks) + elapsed;
            TotalTime = $"{(int)total.TotalHours:00}:{total.Minutes:00}:{total.Seconds:00}";
        }

        private void LoadValues(WorkItem workItem, int? activeSessionId = null)
        {
            Id = workItem.Id;
            ProjectId = workItem.ProjectId;
            Name = workItem.Name;
            Description = workItem.Description;
            IsCompleted = workItem.IsCompleted;
            CreatedOn = workItem.CreatedOn;
            ModifiedOn = workItem.ModifiedOn;

            _baseTicks = workItem.Sessions
                .Where(s => s.Id != activeSessionId)
                .Sum(x => x.Duration.Ticks);
            
            UpdateTime(TimeSpan.Zero);
        }
    }
}