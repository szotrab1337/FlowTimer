using FlowTimer.Domain.Entities;

namespace FlowTimer.Application.Interfaces
{
    public interface IProjectService
    {
        event EventHandler<int>? ProjectArchived;
        event EventHandler<Project>? ProjectCreated;
        Task Archive(int id);
        Task Create(string name, string? description);
        Task<List<Project>> GetAll();
        Task<Project?> GetById(int id);
    }
}