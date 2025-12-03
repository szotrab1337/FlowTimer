using FlowTimer.Domain.Entities;

namespace FlowTimer.Application.Interfaces
{
    public interface ISessionService
    {
        event EventHandler<int>? SessionArchived;
        event EventHandler<Session>? SessionCreated;
        event EventHandler<Session>? SessionEdited;
        Task Archive(int id);
        Task CreateManual(int workItemId, DateTime startTime, DateTime endTime);
        Task Edit(int id, DateTime startTime, DateTime endTime);
        Task<Session?> GetById(int id);
        Task<List<Session>> GetByWorkItemId(int workItemId);
    }
}