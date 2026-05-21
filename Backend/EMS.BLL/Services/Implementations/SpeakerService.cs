// Backend/EMS.BLL/Services/Implementations/SpeakerService.cs
using AutoMapper;
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Speaker;
using EMS.Common.Helpers;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services.Implementations
{
    public class SpeakerService : ISpeakerService
    {
        private readonly IGenericRepository<Speaker> _speakerRepo;
        private readonly IGenericRepository<Session> _sessionRepo;   // ✅ No need for ISessionRepository
        private readonly IMapper _mapper;

        public SpeakerService(
            IGenericRepository<Speaker> speakerRepo,
            IGenericRepository<Session> sessionRepo,   // ✅ Inject generic repo
            IMapper mapper)
        {
            _speakerRepo = speakerRepo;
            _sessionRepo = sessionRepo;
            _mapper = mapper;
        }

        public async Task<SpeakerResponseDto> CreateSpeakerAsync(SpeakerCreateDto dto)
        {
            var entity = _mapper.Map<Speaker>(dto);
            var created = await _speakerRepo.AddAsync(entity);
            return _mapper.Map<SpeakerResponseDto>(created);
        }

        public async Task<bool> DeleteSpeakerAsync(int id)
        {
            var speaker = await _speakerRepo.GetByIdAsync(id);
            if (speaker == null) return false;

            // Remove speaker from all sessions (set SpeakerId to null)
            var sessions = await _sessionRepo.GetQueryable()
                .Where(s => s.SpeakerId == id)
                .ToListAsync();

            foreach (var session in sessions)
            {
                session.SpeakerId = null;
                await _sessionRepo.UpdateAsync(session);
            }

            await _speakerRepo.DeleteAsync(speaker);
            return true;
        }

        public async Task<PagedResult<SpeakerResponseDto>> GetAllSpeakersAsync(PaginationParams pagination)
        {
            var query = _speakerRepo.GetQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
                query = query.Where(s => s.Name.Contains(pagination.SearchTerm));

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.Name)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<SpeakerResponseDto>>(items);
            return new PagedResult<SpeakerResponseDto>
            {
                Items = dtos,
                TotalCount = total,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}