using FlowTimer.Application.Dtos;

namespace FlowTimer.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatistics> GetStatistics();
    }
}