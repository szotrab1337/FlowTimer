using FlowTimer.Domain.Entities;

namespace FlowTimer.Application.Interfaces
{
    public interface IProjectService
    {
        event EventHandler<int>? ProjectArchived;
        event EventHandler<Project>? ProjectCreated;
        event EventHandler<Project>? ProjectEdited;
        Task Archive(int id);
        Task Create(string name, string? description);
        Task Edit(int id, string name, string? description);
        Task<List<Project>> GetAll();
        Task<Project?> GetById(int id);
    }
}