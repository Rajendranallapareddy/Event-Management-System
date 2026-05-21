using System.ComponentModel.DataAnnotations;

namespace EMS.Common.DTOs.Speaker
{
    public class SpeakerCreateDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Company { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;
    }
}