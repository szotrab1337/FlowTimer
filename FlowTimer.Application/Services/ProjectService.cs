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
        public event EventHandler<Project>? ProjectEdited;

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

        public async Task Edit(int id, string name, string? description)
        {
            var project = await _projectRepository.GetById(id);
            if (project is null)
            {
                throw new InvalidOperationException($"Project with ID {id} not found.");
            }

            project.Name = name;
            project.Description = description;

            var result = await _projectRepository.Update(project);

            if (result)
            {
                ProjectEdited?.Invoke(this, project);
            }
        }

        public async Task<List<Project>> GetAll()
        {
            return await _projectRepository.GetAll();
        }

        public async Task<Project?> GetById(int id)
        {
            return await _projectRepository.GetById(id);
        }
    }
}