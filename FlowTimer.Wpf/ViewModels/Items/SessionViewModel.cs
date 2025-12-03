using CommunityToolkit.Mvvm.ComponentModel;
using FlowTimer.Domain.Entities;

namespace FlowTimer.Wpf.ViewModels.Items
{
    public partial class SessionViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime _createdOn;

        [ObservableProperty]
        private DateTime? _endTime;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private bool _isActive;

        [ObservableProperty]
        private bool _isManual;

        [ObservableProperty]
        private DateTime _modifiedOn;

        private Session _session = default!;

        [ObservableProperty]
        private DateTime _startTime;

        [ObservableProperty]
        private string _totalTime = string.Empty;

        [ObservableProperty]
        private int _workItemId;

        public SessionViewModel(Session session)
        {
            LoadValues(session);
        }

        public void UpdateTime(DateTime endTime)
        {
            EndTime = endTime;
            _session.EndTime = endTime;

            TotalTime =
                $"{(int)_session.Duration.TotalHours:00}:{_session.Duration.Minutes:00}:{_session.Duration.Seconds:00}";
        }

        private void LoadValues(Session session)
        {
            _session = session;

            Id = session.Id;
            WorkItemId = session.WorkItemId;
            StartTime = session.StartTime;
            EndTime = session.EndTime;
            IsManual = session.IsManual;
            CreatedOn = session.CreatedOn;
            ModifiedOn = session.ModifiedOn;

            UpdateTime(session.EndTime);
        }
    }
}