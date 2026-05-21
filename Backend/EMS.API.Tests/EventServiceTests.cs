using AutoMapper;
using EMS.BLL.Services.Implementations;
using EMS.Common.DTOs.Event;
using EMS.Common.Helpers;
using EMS.DAL.Models;
using EMS.DAL.Repositories;
using Moq;
using Xunit;

namespace EMS.API.Tests
{
    public class EventServiceTests
    {
        private readonly Mock<IGenericRepository<Event>> _mockRepo;
        private readonly IMapper _mapper;
        private readonly EventService _service;

        public EventServiceTests()
        {
            _mockRepo = new Mock<IGenericRepository<Event>>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventCreateDto, Event>();
                cfg.CreateMap<Event, EventResponseDto>();
            });
            _mapper = config.CreateMapper();
            _service = new EventService(_mockRepo.Object, _mapper);
        }

        [Fact]
        public async Task GetAllEventsAsync_ReturnsPagedResult()
        {
            var events = new List<Event> { new Event { EventId = 1, Title = "Test" } };
            _mockRepo.Setup(r => r.GetQueryable()).Returns(events.AsQueryable());
            var result = await _service.GetAllEventsAsync(new PaginationParams { PageNumber = 1, PageSize = 10 });
            Assert.NotNull(result);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task CreateEventAsync_Valid_ReturnsDto()
        {
            var dto = new EventCreateDto
            {
                Title = "New",
                Description = "Desc",
                Location = "Loc",
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(2),
                Capacity = 100
            };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Event>())).ReturnsAsync(new Event { EventId = 1, Title = dto.Title });
            var result = await _service.CreateEventAsync(dto);
            Assert.Equal("New", result.Title);
        }

        [Fact]
        public async Task DeleteEventAsync_Existing_ReturnsTrue()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Event { EventId = 1 });
            var result = await _service.DeleteEventAsync(1);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteEventAsync_NonExisting_ReturnsFalse()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Event?)null);
            var result = await _service.DeleteEventAsync(999);
            Assert.False(result);
        }
    }
}