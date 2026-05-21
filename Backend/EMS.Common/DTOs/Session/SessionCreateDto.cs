using System.ComponentModel.DataAnnotations;

namespace EMS.Common.DTOs.Session
{
    public class SessionCreateDto
    {
        [Required]
        public int EventId { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        public int? SpeakerId { get; set; }
        [Required]
        public DateTime SessionStart { get; set; }
        [Required]
        public DateTime SessionEnd { get; set; }
    }
}