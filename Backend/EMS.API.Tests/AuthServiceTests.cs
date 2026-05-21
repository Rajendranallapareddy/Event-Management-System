using Xunit;
using Moq;
using AutoMapper;
using EMS.BLL.Services.Implementations;
using EMS.Common.DTOs.User;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace EMS.API.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IGenericRepository<User>> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<User>>();
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("testkey12345678901234567890123456");
            _service = new AuthService(_mockRepo.Object, _mockMapper.Object, _mockConfig.Object);
        }

        [Fact]
        public async Task RegisterAsync_NewUser_ReturnsDto()
        {
            var dto = new RegisterDto { Email = "test@test.com", FullName = "Test", Password = "pass123" };
            _mockRepo.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());
            var user = new User { UserId = 1, Email = dto.Email };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserResponseDto>(user)).Returns(new UserResponseDto { Email = dto.Email });
            var result = await _service.RegisterAsync(dto);
            Assert.NotNull(result);
            Assert.Equal(dto.Email, result.Email);
        }
    }
}