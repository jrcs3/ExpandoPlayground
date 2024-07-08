using Microsoft.EntityFrameworkCore;

namespace ExpandoPlayground;
public class AppDbContext : DbContext
{
    public DbSet<ContactDTO> Contacts { get; set; } // DbSet for ContactDTO

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Additional configuration for the ContactDTO entity can go here
        // For example, to specify a table name:
        modelBuilder.Entity<ContactDTO>().ToTable("Contacts");

        // You can also configure relationships, indexes, etc., here
    }
}
