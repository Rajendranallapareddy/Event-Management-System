using AutoMapper;
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Event;
using EMS.Common.Helpers;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services.Implementations
{
    public class EventService : IEventService
    {
        private readonly IGenericRepository<Event> _eventRepo;
        private readonly IMapper _mapper;

        public EventService(IGenericRepository<Event> eventRepo, IMapper mapper)
        {
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> CreateEventAsync(EventCreateDto dto)
        {
            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End date must be after start date.");

            var entity = _mapper.Map<Event>(dto);
            var created = await _eventRepo.AddAsync(entity);
            return _mapper.Map<EventResponseDto>(created);
        }

        public async Task<EventResponseDto?> UpdateEventAsync(int id, EventCreateDto dto)
        {
            var existing = await _eventRepo.GetByIdAsync(id);
            if (existing == null) return null;

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End date must be after start date.");

            _mapper.Map(dto, existing);
            await _eventRepo.UpdateAsync(existing);
            return _mapper.Map<EventResponseDto>(existing);
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var entity = await _eventRepo.GetByIdAsync(id);
            if (entity == null) return false;
            await _eventRepo.DeleteAsync(entity);
            return true;
        }

        public async Task<EventResponseDto?> GetEventByIdAsync(int id)
        {
            var evt = await _eventRepo.GetQueryable()
                .Include(e => e.Sessions)
                .FirstOrDefaultAsync(e => e.EventId == id);
            if (evt == null) return null;
            return new EventResponseDto
            {
                EventId = evt.EventId,
                Title = evt.Title,
                Description = evt.Description,
                Location = evt.Location,
                StartDate = evt.StartDate,
                EndDate = evt.EndDate,
                Capacity = evt.Capacity,
                ImageUrl = evt.ImageUrl,
                SessionCount = evt.Sessions.Count
            };
        }

        public async Task<PagedResult<EventResponseDto>> GetAllEventsAsync(PaginationParams pagination)
        {
            var query = _eventRepo.GetQueryable();
            query = query.Include(e => e.Sessions);

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
                query = query.Where(e => e.Title.Contains(pagination.SearchTerm) || e.Description.Contains(pagination.SearchTerm));

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(e => e.StartDate)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = items.Select(e => new EventResponseDto
            {
                EventId = e.EventId,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Capacity = e.Capacity,
                ImageUrl = e.ImageUrl,
                SessionCount = e.Sessions.Count
            }).ToList();

            return new PagedResult<EventResponseDto>
            {
                Items = dtos,
                TotalCount = total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}