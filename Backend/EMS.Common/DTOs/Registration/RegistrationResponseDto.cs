namespace EMS.Common.DTOs.Registration
{
    public class RegistrationResponseDto
    {
        public int RegistrationId { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string? ParticipantEmail { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool AttendanceStatus { get; set; }
    }
}