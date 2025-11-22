using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;

namespace FlowTimer.Application.Services
{
    public class ProjectService(
        IProjectRepository projectRepository) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public event EventHandler<int>? ProjectArchived;
        public event EventHandler<Project>? ProjectCreated;

        public async Task Archive(int id)
        {
            var result = await _projectRepository.Archive(id);

            if (result)
            {
                ProjectArchived?.Invoke(this, id);
            }
        }

        public async Task Create(string name, string? description)
        {
            var project = new Project
            {
                Name = name,
                Description = description
            };

            var result = await _projectRepository.Add(project);

            if (result)
            {
                ProjectCreated?.Invoke(this, project);
            }
        }

        public async Task<List<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }
    }
}