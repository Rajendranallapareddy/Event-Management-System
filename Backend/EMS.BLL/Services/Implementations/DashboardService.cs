using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Dashboard;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IGenericRepository<Event> _eventRepo;
        private readonly IGenericRepository<Speaker> _speakerRepo;
        private readonly IGenericRepository<Registration> _regRepo;
        private readonly IGenericRepository<User> _userRepo;
        
        public DashboardService(IGenericRepository<Event> eventRepo, IGenericRepository<Speaker> speakerRepo, IGenericRepository<Registration> regRepo, IGenericRepository<User> userRepo)
        {
            _eventRepo = eventRepo;
            _speakerRepo = speakerRepo;
            _regRepo = regRepo;
            _userRepo = userRepo;
        }
        
        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            return new DashboardStatsDto
            {
                TotalEvents = await _eventRepo.GetQueryable().CountAsync(),
                TotalSpeakers = await _speakerRepo.GetQueryable().CountAsync(),
                TotalRegistrations = await _regRepo.GetQueryable().CountAsync(),
                TotalParticipants = await _userRepo.GetQueryable().Where(u => u.Role == "Participant").CountAsync()
            };
        }
    }
}