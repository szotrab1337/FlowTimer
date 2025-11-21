using FlowTimer.Domain.Entities;

namespace FlowTimer.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetAll();
    }
}