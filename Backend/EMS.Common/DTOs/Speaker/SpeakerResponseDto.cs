namespace EMS.Common.DTOs.Speaker
{
    public class SpeakerResponseDto
    {
        public int SpeakerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
    }
}