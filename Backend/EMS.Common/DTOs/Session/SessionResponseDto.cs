namespace EMS.Common.DTOs.Session
{
    public class SessionResponseDto
    {
        public int SessionId { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? SpeakerId { get; set; }
        public string? SpeakerName { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
    }
}