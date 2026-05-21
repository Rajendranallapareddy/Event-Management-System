using AutoMapper;
using EMS.BLL.Services.Interfaces;
using EMS.Common.DTOs.User;
using EMS.Common.Helpers;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserResponseDto>> GetAllUsersAsync(PaginationParams pagination)
        {
            var query = _userRepo.GetQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
                // ✅ Use FullName (not UserName)
                query = query.Where(u => u.FullName.Contains(pagination.SearchTerm) || u.Email.Contains(pagination.SearchTerm));

            var total = await query.CountAsync();
            var items = await query
                // ✅ Order by FullName
                .OrderBy(u => u.FullName)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<UserResponseDto>>(items);
            return new PagedResult<UserResponseDto> { Items = dtos, TotalCount = total, PageNumber = pagination.PageNumber, PageSize = pagination.PageSize };
        }
    }
}