// Backend/EMS.DAL/Repositories/ISessionRepository.cs
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using System.Linq.Expressions;

namespace EMS.DAL.Repositories
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetSessionsByEventAsync(int eventId);
        Task<bool> AssignSpeakerAsync(int sessionId, int speakerId);
        Task<IEnumerable<Session>> GetUpcomingSessionsAsync();
    }
}