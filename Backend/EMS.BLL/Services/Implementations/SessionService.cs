using AutoMapper;
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Session;
using EMS.Common.Helpers;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services.Implementations
{
    public class SessionService : ISessionService
    {
        private readonly IGenericRepository<Session> _sessionRepo;
        private readonly IGenericRepository<Speaker> _speakerRepo;
        private readonly IGenericRepository<Event> _eventRepo;
        private readonly IMapper _mapper;
        
        public SessionService(IGenericRepository<Session> sessionRepo, IGenericRepository<Speaker> speakerRepo, IGenericRepository<Event> eventRepo, IMapper mapper)
        {
            _sessionRepo = sessionRepo;
            _speakerRepo = speakerRepo;
            _eventRepo = eventRepo;
            _mapper = mapper;
        }
        
        public async Task<SessionResponseDto> CreateSessionAsync(SessionCreateDto dto)
        {
            var eventExists = await _eventRepo.ExistsAsync(e => e.EventId == dto.EventId);
            if (!eventExists) throw new ArgumentException("Event does not exist.");
            if (dto.SessionEnd <= dto.SessionStart)
                throw new ArgumentException("Session end must be after start.");
            var entity = _mapper.Map<Session>(dto);
            var created = await _sessionRepo.AddAsync(entity);
            return _mapper.Map<SessionResponseDto>(created);
        }
        
        public async Task<bool> DeleteSessionAsync(int id)
        {
            var entity = await _sessionRepo.GetByIdAsync(id);
            if (entity == null) return false;
            await _sessionRepo.DeleteAsync(entity);
            return true;
        }
        
        public async Task<bool> AssignSpeakerAsync(int sessionId, int speakerId)
        {
            var session = await _sessionRepo.GetByIdAsync(sessionId);
            if (session == null) return false;
            var speaker = await _speakerRepo.GetByIdAsync(speakerId);
            if (speaker == null) return false;
            session.SpeakerId = speakerId;
            await _sessionRepo.UpdateAsync(session);
            return true;
        }
        public async Task<PagedResult<SessionResponseDto>> GetAllSessionsAsync(PaginationParams pagination)
{
    var query = _sessionRepo.GetQueryable()
        .Include(s => s.Event)
        .Include(s => s.Speaker);

    var total = await query.CountAsync();
    var items = await query
        .OrderBy(s => s.SessionStart)
        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
        .Take(pagination.PageSize)
        .ToListAsync();

    var dtos = _mapper.Map<List<SessionResponseDto>>(items);
    return new PagedResult<SessionResponseDto>
    {
        Items = dtos,
        TotalCount = total,
        PageNumber = pagination.PageNumber,
        PageSize = pagination.PageSize
    };
}
    }
}