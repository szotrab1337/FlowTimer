namespace FlowTimer.Domain.Entities
{
    public class WorkItem
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }

        public Project Project { get; set; } = default!;
        public ICollection<Session> Sessions { get; set; } = [];

        public TimeSpan Duration => TimeSpan.FromTicks(Sessions.Sum(x => x.Duration.Ticks));
    }
}