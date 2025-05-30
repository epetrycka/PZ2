using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace MeetLab.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MeetLab.Models.User> Users { get; set; }

        public DbSet<MeetLab.Models.Friendship> Friendship { get; set; }

        public DbSet<MeetLab.Models.Message> Messages { get; set; }

        public DbSet<MeetLab.Models.Post> Posts { get; set; }

        public DbSet<MeetLab.Models.Comment> Comments { get; set; }

        public DbSet<MeetLab.Models.UserProfile> UserProfiles { get; set; }

        //dotnet ef migrations add Update
        //dotnet ef database update

        public static string GenerateToken()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MeetLab.Models.User>()
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFriends"));

            modelBuilder.Entity<MeetLab.Models.User>().HasData(
                new MeetLab.Models.User { NickName = "admin", FirstName = "Admin", Password = "E64B78FC3BC91BCBC7DC232BA8EC59E0", Token = MeetLab.Credentials.AdminCredentials.AdminToken, RegistrationDate = DateTime.Now }
            );
        }
    }
}