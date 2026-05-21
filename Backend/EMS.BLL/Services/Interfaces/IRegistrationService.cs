using EMS.Common.DTOs.Registration;
using EMS.Common.Helpers;

namespace EMS.BLL.Services.Interfaces
{
    public interface IRegistrationService
    {
        Task<RegistrationResponseDto> RegisterForEventAsync(int userId, int eventId);
        Task<bool> CancelRegistrationAsync(int registrationId);
        Task<bool> MarkAttendanceAsync(int registrationId);
        Task<PagedResult<RegistrationResponseDto>> GetRegistrationsByUserAsync(int userId, PaginationParams pagination);
        Task<PagedResult<RegistrationResponseDto>> GetAllRegistrationsAsync(PaginationParams pagination);
    }
}