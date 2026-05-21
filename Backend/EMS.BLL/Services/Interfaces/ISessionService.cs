using EMS.Common.DTOs.Session;
using EMS.Common.Helpers;

namespace EMS.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<SessionResponseDto> CreateSessionAsync(SessionCreateDto dto);
        Task<bool> DeleteSessionAsync(int id);
        Task<bool> AssignSpeakerAsync(int sessionId, int speakerId);
        Task<PagedResult<SessionResponseDto>> GetAllSessionsAsync(PaginationParams pagination);
    }
}