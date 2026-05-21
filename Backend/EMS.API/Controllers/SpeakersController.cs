// Backend/EMS.API/Controllers/SpeakersController.cs
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.Speaker;
using EMS.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpeakersController : ControllerBase
    {
        private readonly ISpeakerService _speakerService;
        public SpeakersController(ISpeakerService speakerService) => _speakerService = speakerService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams pagination)
        {
            var result = await _speakerService.GetAllSpeakersAsync(pagination);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SpeakerCreateDto dto)
        {
            var created = await _speakerService.CreateSpeakerAsync(dto);
            return Ok(created);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _speakerService.DeleteSpeakerAsync(id);
            if (!deleted) return NotFound();
            return NoContent();   // ✅ 204 No Content on success
        }
    }
}