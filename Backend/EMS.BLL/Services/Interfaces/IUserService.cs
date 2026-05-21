using EMS.Common.DTOs.User;
using EMS.Common.Helpers;

namespace EMS.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserResponseDto>> GetAllUsersAsync(PaginationParams pagination);
    }
}