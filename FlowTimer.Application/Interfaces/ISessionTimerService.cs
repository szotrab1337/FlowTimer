namespace FlowTimer.Application.Interfaces
{
    public interface ISessionTimerService
    {
        event EventHandler<SessionStartedEventArgs>? SessionStarted;
        event EventHandler<SessionStoppedEventArgs>? SessionStopped;
        event EventHandler<SessionTimerTickEventArgs>? Tick;

        int? ActiveWorkItemId { get; }
        int? ActiveProjectId { get; }
        bool IsRunning { get; }
        int? ActiveSessionId { get; }

        Task Start(int projectId, int workItemId);
        Task Stop();
    }

    public class SessionTimerTickEventArgs(int projectId, int workItemId, TimeSpan elapsed)
    {
        public int ProjectId { get; } = projectId;
        public int WorkItemId { get; } = workItemId;
        public TimeSpan Elapsed { get; } = elapsed;
    }

    public class SessionStartedEventArgs(int projectId, int workItemId, int sessionId)
    {
        public int ProjectId { get; } = projectId;
        public int WorkItemId { get; } = workItemId;
        public int SessionId { get; } = sessionId;
    }

    public class SessionStoppedEventArgs(int projectId, int workItemId, int sessionId, TimeSpan duration)
    {
        public int ProjectId { get; } = projectId;
        public int WorkItemId { get; } = workItemId;
        public int SessionId { get; } = sessionId;
        public TimeSpan Duration { get; } = duration;
    }
}