using FlowTimer.Domain.Entities;

namespace FlowTimer.Application.Interfaces
{
    public interface IWorkItemService
    {
        event EventHandler<int>? WorkItemArchived;
        event EventHandler<WorkItem>? WorkItemCreated;
        event EventHandler<WorkItem>? WorkItemEdited;
        Task Archive(int id);
        Task Create(int projectId, string name, string? description);
        Task Edit(int id, string name, string? description, bool isCompleted);
        Task<WorkItem?> GetById(int id);
        Task<List<WorkItem>> GetByProjectId(int projectId);
    }
}