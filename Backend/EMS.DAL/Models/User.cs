using System.ComponentModel.DataAnnotations;

namespace EMS.DAL.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = "Participant";
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}