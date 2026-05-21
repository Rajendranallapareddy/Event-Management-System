namespace EMS.Common.DTOs.Event
{
    public class EventResponseDto
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
        public int SessionCount { get; set; }
    }
}