using FlowTimer.Domain.Entities;

namespace FlowTimer.Domain.Repositories
{
    public interface ISessionRepository
    {
        Task<bool> Add(Session session);
        Task<bool> Archive(int id);
        Task<Session?> GetById(int id);
        Task<List<Session>> GetByWorkItemId(int workItemId);
        Task<bool> Update(Session session);
    }
}