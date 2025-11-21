using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;

namespace FlowTimer.Application.Services
{
    public class ProjectService(IProjectRepository projectRepository) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<List<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }
    }
}