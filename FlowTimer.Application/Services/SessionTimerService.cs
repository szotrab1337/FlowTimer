using System.Timers;
using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;
using Timer = System.Timers.Timer;

namespace FlowTimer.Application.Services
{
    public class SessionTimerService(
        ISessionRepository sessionRepository) : ISessionTimerService
    {
        private readonly Timer _saveTimer = new(TimeSpan.FromSeconds(5));
        private readonly ISessionRepository _sessionRepository = sessionRepository;
        private readonly Timer _uiTimer = new(TimeSpan.FromSeconds(1));

        private Session? _activeSession;
        private int? _activeWorkItemId;
        private int? _activeProjectId;
        private DateTime _startTime;

        public event EventHandler<SessionStartedEventArgs>? SessionStarted;
        public event EventHandler<SessionStoppedEventArgs>? SessionStopped;
        public event EventHandler<SessionTimerTickEventArgs>? Tick;

        public int? ActiveWorkItemId => _activeWorkItemId;
        public int? ActiveProjectId => _activeProjectId;
        public bool IsRunning => _activeSession is not null;

        public void Dispose()
        {
            _uiTimer.Dispose();
            _saveTimer.Dispose();
        }

        public async Task Start(int projectId, int workItemId)
        {
            // If a session is already running, stop it first
            if (IsRunning)
            {
                await Stop();
            }

            _startTime = DateTime.Now;
            _activeWorkItemId = workItemId;
            _activeProjectId = projectId;

            _activeSession = new Session
            {
                WorkItemId = workItemId,
                StartTime = _startTime,
                EndTime = null,
                IsManual = false
            };

            await _sessionRepository.Add(_activeSession);

            _uiTimer.Elapsed += OnUiTimerElapsed;
            _uiTimer.Start();

            _saveTimer.Elapsed += OnSaveTimerElapsed;
            _saveTimer.Start();

            SessionStarted?.Invoke(this, new SessionStartedEventArgs(projectId, workItemId, _activeSession.Id));
        }

        public async Task Stop()
        {
            if (_activeSession is null || _activeWorkItemId is null || _activeProjectId is null)
            {
                return;
            }

            _uiTimer.Stop();
            _uiTimer.Elapsed -= OnUiTimerElapsed;

            _saveTimer.Stop();
            _saveTimer.Elapsed -= OnSaveTimerElapsed;

            var endTime = DateTime.Now;
            var duration = endTime - _startTime;

            await _sessionRepository.UpdateEndTime(_activeSession.Id, endTime);

            var projectId = _activeProjectId.Value;
            var workItemId = _activeWorkItemId.Value;
            var sessionId = _activeSession.Id;

            _activeProjectId = null;
            _activeWorkItemId = null;
            _activeSession = null;

            SessionStopped?.Invoke(this, new SessionStoppedEventArgs(projectId, workItemId, sessionId, duration));
        }

        private async void OnSaveTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_activeSession is null)
            {
                return;
            }

            await _sessionRepository.UpdateEndTime(_activeSession.Id, DateTime.Now);
        }

        private void OnUiTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_activeWorkItemId is null || _activeProjectId is null)
            {
                return;
            }

            var elapsed = DateTime.Now - _startTime;
            Tick?.Invoke(this, new SessionTimerTickEventArgs(_activeProjectId.Value, _activeWorkItemId.Value, elapsed));
        }
    }
}