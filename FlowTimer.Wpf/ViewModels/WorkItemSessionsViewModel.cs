using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Wpf.Dialogs;
using FlowTimer.Wpf.Navigation;
using FlowTimer.Wpf.ViewModels.Items;
using FlowTimer.Wpf.Views;
using Microsoft.Extensions.Logging;

namespace FlowTimer.Wpf.ViewModels
{
    public partial class WorkItemSessionsViewModel(
        ISessionService sessionService,
        INavigationService navigationService,
        ISessionTimerService sessionTimerService,
        ILogger<WorkItemSessionsViewModel> logger) : ObservableObject
    {
        private readonly ILogger<WorkItemSessionsViewModel> _logger = logger;
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISessionService _sessionService = sessionService;
        private readonly ISessionTimerService _sessionTimerService = sessionTimerService;

        [ObservableProperty]
        private ObservableCollection<SessionViewModel> _sessions = [];

        private WorkItem _workItem = default!;

        public void Cleanup()
        {
            _sessionTimerService.SessionStopped -= OnSessionStopped;
            _sessionTimerService.Tick -= OnSessionTimerTick;
            _sessionService.SessionArchived -= OnSessionArchived;
        }

        public async Task Initialize(WorkItem workItem)
        {
            _workItem = workItem;

            await LoadSessions();

            if (_sessionTimerService.IsRunning)
            {
                var session = Sessions.FirstOrDefault(x => x.Id == _sessionTimerService.ActiveSessionId);
                session?.IsActive = true;
            }

            _sessionTimerService.SessionStopped += OnSessionStopped;
            _sessionTimerService.Tick += OnSessionTimerTick;
            _sessionService.SessionArchived += OnSessionArchived;
        }

        [RelayCommand]
        private void AddSession()
        {
            _navigationService.Navigate(typeof(AddSessionPage), _workItem.Id);
        }

        [RelayCommand]
        private async Task ArchiveSession(SessionViewModel vm)
        {
            try
            {
                var confirmed = await FluentMessageBox.Confirm(
                    $"Czy na pewno chcesz zarchiwizować sesję '{vm.StartTime:g} - {vm.EndTime:g}'?");

                if (confirmed)
                {
                    await _sessionService.Archive(vm.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while archiving session.");
            }
        }

        [RelayCommand]
        private void EditSession(SessionViewModel vm)
        {
            _navigationService.Navigate(typeof(EditSessionPage), vm.Id);
        }

        [RelayCommand]
        private void GoBack()
        {
            _navigationService.Navigate(typeof(ProjectDashboardPage), _workItem.ProjectId);
        }

        private async Task LoadSessions()
        {
            var sessions = await _sessionService.GetByWorkItemId(_workItem.Id);
            var vms = sessions.Select(x => new SessionViewModel(x));

            Sessions = new ObservableCollection<SessionViewModel>(vms);
        }

        private void OnSessionArchived(object? sender, int e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var session = Sessions.FirstOrDefault(x => x.Id == e);

                if (session is not null)
                {
                    Sessions.Remove(session);
                }
            });
        }

        private void OnSessionStopped(object? sender, SessionStoppedEventArgs e)
        {
            var session = Sessions.FirstOrDefault(x => x.IsActive);
            session?.IsActive = false;
        }

        private void OnSessionTimerTick(object? sender, SessionTimerTickEventArgs e)
        {
            var session = Sessions.FirstOrDefault(x => x.Id == e.SessionId);
            session?.UpdateTime(e.EndTime);
        }
    }
}