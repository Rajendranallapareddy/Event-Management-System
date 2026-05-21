using System.ComponentModel.DataAnnotations;

namespace EMS.Common.DTOs.Event
{
    public class EventCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required, MaxLength(200)]
        public string Location { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
    }
}