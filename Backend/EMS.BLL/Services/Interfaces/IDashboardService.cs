using EMS.Common.DTOs.Dashboard;

namespace EMS.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
    }
}