using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Session;
using EMS.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService sessionService) => _sessionService = sessionService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var result = await _sessionService.GetAllSessionsAsync(pagination);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SessionCreateDto dto)
        {
            var created = await _sessionService.CreateSessionAsync(dto);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{sessionId}/assign-speaker/{speakerId}")]
        public async Task<IActionResult> AssignSpeaker(int sessionId, int speakerId)
        {
            var success = await _sessionService.AssignSpeakerAsync(sessionId, speakerId);
            if (!success) return NotFound();
            return Ok(new { message = "Speaker assigned successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _sessionService.DeleteSessionAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}