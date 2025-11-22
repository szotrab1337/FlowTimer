using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Entities;
using FlowTimer.Domain.Repositories;

namespace FlowTimer.Application.Services
{
    public class WorkItemService(
        IWorkItemRepository workItemRepository) : IWorkItemService
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;

        public event EventHandler<int>? WorkItemArchived;
        public event EventHandler<WorkItem>? WorkItemCreated;
        public event EventHandler<WorkItem>? WorkItemEdited;

        public async Task Archive(int id)
        {
            var result = await _workItemRepository.Archive(id);

            if (result)
            {
                WorkItemArchived?.Invoke(this, id);
            }
        }

        public async Task Create(int projectId, string name, string? description)
        {
            var workItem = new WorkItem
            {
                ProjectId = projectId,
                Name = name,
                Description = description
            };

            var result = await _workItemRepository.Add(workItem);

            if (result)
            {
                WorkItemCreated?.Invoke(this, workItem);
            }
        }

        public async Task Edit(int id, string name, string? description, bool isCompleted)
        {
            var workItem = await _workItemRepository.GetById(id);
            if (workItem is null)
            {
                throw new InvalidOperationException($"Work item with ID {id} not found.");
            }

            workItem.Name = name;
            workItem.Description = description;
            workItem.IsCompleted = isCompleted;

            var result = await _workItemRepository.Update(workItem);

            if (result)
            {
                WorkItemEdited?.Invoke(this, workItem);
            }
        }

        public async Task<WorkItem?> GetById(int id)
        {
            return await _workItemRepository.GetById(id);
        }

        public async Task<List<WorkItem>> GetByProjectId(int projectId)
        {
            return await _workItemRepository.GetByProjectId(projectId);
        }
    }
}