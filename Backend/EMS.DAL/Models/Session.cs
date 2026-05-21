using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.DAL.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        
        [Required]
        public int EventId { get; set; }
        
        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
        
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        public int? SpeakerId { get; set; }
        
        [ForeignKey(nameof(SpeakerId))]
        public virtual Speaker? Speaker { get; set; }
        
        [Required]
        public DateTime SessionStart { get; set; }
        
        [Required]
        public DateTime SessionEnd { get; set; }
    }
}