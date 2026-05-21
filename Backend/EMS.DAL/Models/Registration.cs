using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.DAL.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
        
        [Required]
        public int EventId { get; set; }
        
        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
        
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        
        public bool AttendanceStatus { get; set; } = false;
    }
}