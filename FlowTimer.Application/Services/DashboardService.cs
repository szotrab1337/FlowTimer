using FlowTimer.Application.Dtos;
using FlowTimer.Application.Interfaces;
using FlowTimer.Domain.Repositories;

namespace FlowTimer.Application.Services
{
    public class DashboardService(
        IProjectRepository projectRepository) : IDashboardService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<DashboardStatistics> GetStatistics()
        {
            var projects = await _projectRepository.GetAll();
            var allWorkItems = projects.SelectMany(p => p.WorkItems).ToList();
            var allSessions = allWorkItems.SelectMany(w => w.Sessions).ToList();

            return new DashboardStatistics
            {
                ProjectsCount = projects.Count,
                PendingTasksCount = allWorkItems.Count(w => !w.IsCompleted),
                TotalTime = TimeSpan.FromTicks(allWorkItems.Sum(w => w.Duration.Ticks)),
                SessionsCount = allSessions.Count
            };
        }
    }
}