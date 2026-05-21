// Backend/EMS.BLL/Services/Interfaces/ISpeakerService.cs
using EMS.Common.DTOs.Speaker;
using EMS.Common.Helpers;

namespace EMS.BLL.Services.Interfaces
{
    public interface ISpeakerService
    {
        Task<SpeakerResponseDto> CreateSpeakerAsync(SpeakerCreateDto dto);
        Task<bool> DeleteSpeakerAsync(int id);
        Task<PagedResult<SpeakerResponseDto>> GetAllSpeakersAsync(PaginationParams pagination);
    }
}