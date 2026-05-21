using EMS.Common.DTOs.User;

namespace EMS.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto?> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
        Task<UserResponseDto?> GetUserByEmailAsync(string email);
    }
}