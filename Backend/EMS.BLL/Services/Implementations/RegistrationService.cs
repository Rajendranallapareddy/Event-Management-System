using AutoMapper;
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Registration;
using EMS.Common.Helpers;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IGenericRepository<Registration> _regRepo;
        private readonly IGenericRepository<Event> _eventRepo;
        private readonly IMapper _mapper;

        public RegistrationService(IGenericRepository<Registration> regRepo, IGenericRepository<Event> eventRepo, IMapper mapper)
        {
            _regRepo = regRepo;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }

        public async Task<RegistrationResponseDto> RegisterForEventAsync(int userId, int eventId)
        {
            var existing = await _regRepo.FindAsync(r => r.UserId == userId && r.EventId == eventId);
            if (existing.Any()) throw new InvalidOperationException("Already registered for this event.");
            var eventEntity = await _eventRepo.GetByIdAsync(eventId);
            if (eventEntity == null) throw new ArgumentException("Event not found.");
            var registration = new Registration
            {
                UserId = userId,
                EventId = eventId,
                RegistrationDate = DateTime.UtcNow,
                AttendanceStatus = false
            };
            var created = await _regRepo.AddAsync(registration);
            return new RegistrationResponseDto
            {
                RegistrationId = created.RegistrationId,
                EventId = created.EventId,
                EventTitle = eventEntity.Title,
                RegistrationDate = created.RegistrationDate,
                AttendanceStatus = created.AttendanceStatus
            };
        }

        public async Task<bool> CancelRegistrationAsync(int registrationId)
        {
            var reg = await _regRepo.GetByIdAsync(registrationId);
            if (reg == null) return false;
            await _regRepo.DeleteAsync(reg);
            return true;
        }

        public async Task<bool> MarkAttendanceAsync(int registrationId)
        {
            var reg = await _regRepo.GetByIdAsync(registrationId);
            if (reg == null) return false;
            reg.AttendanceStatus = true;
            await _regRepo.UpdateAsync(reg);
            return true;
        }

        // For participant: get registrations by user
        public async Task<PagedResult<RegistrationResponseDto>> GetRegistrationsByUserAsync(int userId, PaginationParams pagination)
        {
            var query = _regRepo.GetQueryable()
                .Where(r => r.UserId == userId)
                .Include(r => r.Event)
                .OrderByDescending(r => r.RegistrationDate);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = items.Select(r => new RegistrationResponseDto
            {
                RegistrationId = r.RegistrationId,
                EventId = r.EventId,
                EventTitle = r.Event.Title,
                RegistrationDate = r.RegistrationDate,
                AttendanceStatus = r.AttendanceStatus
            }).ToList();

            return new PagedResult<RegistrationResponseDto> { Items = dtos, TotalCount = total, PageNumber = pagination.PageNumber, PageSize = pagination.PageSize };
        }

        // For admin: get all registrations
        public async Task<PagedResult<RegistrationResponseDto>> GetAllRegistrationsAsync(PaginationParams pagination)
        {
            var query = _regRepo.GetQueryable()
                .Include(r => r.Event)
                .Include(r => r.User)
                .OrderByDescending(r => r.RegistrationDate);

            var total = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = items.Select(r => new RegistrationResponseDto
            {
                RegistrationId = r.RegistrationId,
                EventId = r.EventId,
                EventTitle = r.Event.Title,
                ParticipantEmail = r.User.Email,
                RegistrationDate = r.RegistrationDate,
                AttendanceStatus = r.AttendanceStatus
            }).ToList();

            return new PagedResult<RegistrationResponseDto> { Items = dtos, TotalCount = total, PageNumber = pagination.PageNumber, PageSize = pagination.PageSize };
        }
    }
}