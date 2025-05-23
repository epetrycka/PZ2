using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Login> Loginy { get; set; }
    public DbSet<Dane> Dane { get; set; }

    //dotnet ef migrations add Update
    //dotnet ef database update
    // Admin123 = 0192023a7bbd73250516f069df18b500
    // User1234 = 4b4f0b7e5f5f22ad4955d1e43416d20c

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Login>().HasData(
            new Login { Id = 1, LoginName = "admin", Password = "E64B78FC3BC91BCBC7DC232BA8EC59E0" },
            new Login { Id = 2, LoginName = "user", Password = "6EDF26F6E0BADFF12FCA32B16DB38BF2" }
        );

        modelBuilder.Entity<Dane>().HasData(
            new Dane { Id = 1, Tekst = "To jest pierwszy komentarz." },
            new Dane { Id = 2, Tekst = "Drugi przykładowy wpis." },
            new Dane { Id = 3, Tekst = "Trzeci tekst testowy." }
        );
    }
}
