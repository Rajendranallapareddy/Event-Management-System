using EMS.BLL.Services.Interfaces;
using EMS.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        public RegistrationsController(IRegistrationService registrationService) => _registrationService = registrationService;

        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out int id) ? id : 0;
        }

        [HttpPost("register/{eventId}")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> RegisterForEvent(int eventId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            try
            {
                var result = await _registrationService.RegisterForEventAsync(userId, eventId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("my-events")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyEvents([FromQuery] PaginationParams pagination)
        {
            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();
            var result = await _registrationService.GetRegistrationsByUserAsync(userId, pagination);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("attendance/{registrationId}")]
        public async Task<IActionResult> MarkAttendance(int registrationId)
        {
            var success = await _registrationService.MarkAttendanceAsync(registrationId);
            if (!success) return NotFound();
            return Ok(new { message = "Attendance marked" });
        }

        [Authorize(Roles = "Participant")]
        [HttpDelete("cancel/{registrationId}")]
        public async Task<IActionResult> CancelRegistration(int registrationId)
        {
            var success = await _registrationService.CancelRegistrationAsync(registrationId);
            if (!success) return NotFound();
            return NoContent();
        }

       [Authorize(Roles = "Admin")]
       [HttpGet("admin/all")]
        public async Task<IActionResult> GetAllRegistrations([FromQuery] PaginationParams pagination)
        {
            var result = await _registrationService.GetAllRegistrationsAsync(pagination);
            return Ok(result);
        }
    }
}