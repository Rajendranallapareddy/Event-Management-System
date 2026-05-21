using Microsoft.EntityFrameworkCore;
using EMS.DAL.Models;
using BCrypt.Net;

namespace EMS.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Speaker> Speakers => Set<Speaker>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Registration> Registrations => Set<Registration>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed admin user
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = 1,
                FullName = "System Admin",
                Email = "admin@ems.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                CreatedDate = DateTime.UtcNow
            });
            
            // Seed demo events, speakers, sessions (optional)
            modelBuilder.Entity<Event>().HasData(
                new Event { EventId = 1, Title = "Tech Summit 2025", Description = "Annual technology conference", Location = "Bangalore", StartDate = DateTime.UtcNow.AddDays(30), EndDate = DateTime.UtcNow.AddDays(32), Capacity = 200, ImageUrl = "https://picsum.photos/300/200" },
                new Event { EventId = 2, Title = "AI Workshop", Description = "Hands-on AI/ML workshop", Location = "Online", StartDate = DateTime.UtcNow.AddDays(15), EndDate = DateTime.UtcNow.AddDays(16), Capacity = 50 }
            );
            
            modelBuilder.Entity<Speaker>().HasData(
                new Speaker { SpeakerId = 1, Name = "Dr. John Smith", Bio = "AI Expert", Company = "TechCorp", Designation = "Principal Scientist" }
            );
            
            modelBuilder.Entity<Session>().HasData(
                new Session { SessionId = 1, EventId = 1, Title = "Keynote", Description = "Opening keynote", SpeakerId = 1, SessionStart = DateTime.UtcNow.AddDays(30).AddHours(9), SessionEnd = DateTime.UtcNow.AddDays(30).AddHours(10) }
            );
        }
    }
}