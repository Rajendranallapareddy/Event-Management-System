using EMS.BLL.Services.Interfaces;
using EMS.Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] PaginationParams pagination)
        {
            var result = await _userService.GetAllUsersAsync(pagination);
            return Ok(result);
        }
    }
}