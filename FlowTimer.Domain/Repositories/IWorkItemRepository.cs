using FlowTimer.Domain.Entities;

namespace FlowTimer.Domain.Repositories
{
    public interface IWorkItemRepository
    {
        Task<bool> Add(WorkItem workItem);
        Task<bool> Archive(int id);
        Task<WorkItem?> GetById(int id);
        Task<List<WorkItem>> GetByProjectId(int projectId);
        Task<bool> Update(WorkItem workItem);
    }
}