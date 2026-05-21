using AutoMapper;
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.User;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMS.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthService(IGenericRepository<User> userRepo, IMapper mapper, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _config = config;
        }

        public async Task<UserResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepo.FindAsync(u => u.Email == dto.Email);
            if (existing.Any())
                throw new InvalidOperationException("Email already registered");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Participant",
                CreatedDate = DateTime.UtcNow
            };
            var created = await _userRepo.AddAsync(user);
            return _mapper.Map<UserResponseDto>(created);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var users = await _userRepo.FindAsync(u => u.Email == dto.Email);
            var user = users.FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var users = await _userRepo.FindAsync(u => u.Email == email);
            var user = users.FirstOrDefault();
            return user == null ? null : _mapper.Map<UserResponseDto>(user);
        }
    }
}