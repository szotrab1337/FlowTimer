namespace FlowTimer.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public int WorkItemId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsManual { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }

        public WorkItem WorkItem { get; set; } = default!;
        
        public TimeSpan Duration => (EndTime ?? DateTime.UtcNow) - StartTime;
    }
}