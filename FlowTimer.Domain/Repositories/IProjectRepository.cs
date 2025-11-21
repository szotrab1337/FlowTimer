using FlowTimer.Domain.Entities;

namespace FlowTimer.Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<bool> Add(Project project);
        Task<bool> Archive(int id);
        Task<List<Project>> GetAll();
        Task<Project?> GetById(int id);
        Task<bool> Update(Project project);
    }
}