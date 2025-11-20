namespace FlowTimer.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? ArchivedOn { get; set; }
        
        public ICollection<WorkItem> WorkItems { get; set; } = [];
    }
}