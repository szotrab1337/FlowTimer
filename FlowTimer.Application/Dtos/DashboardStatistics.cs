namespace FlowTimer.Application.Dtos
{
    public class DashboardStatistics
    {
        public int ProjectsCount { get; set; }
        public int PendingTasksCount { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int SessionsCount { get; set; }
    }
}