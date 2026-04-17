using Core.Dtos;

namespace Core.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync(string userId);
}