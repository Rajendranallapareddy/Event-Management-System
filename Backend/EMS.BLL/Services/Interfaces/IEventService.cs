using EMS.Common.DTOs.Event;
using EMS.Common.Helpers;

namespace EMS.BLL.Services.Interfaces
{
    public interface IEventService
    {
        Task<EventResponseDto> CreateEventAsync(EventCreateDto dto);
        Task<EventResponseDto?> UpdateEventAsync(int id, EventCreateDto dto);
        Task<bool> DeleteEventAsync(int id);
        Task<EventResponseDto?> GetEventByIdAsync(int id);
        Task<PagedResult<EventResponseDto>> GetAllEventsAsync(PaginationParams pagination);
    }
}