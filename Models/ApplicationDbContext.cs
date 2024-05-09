using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshop.Models;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TicketModel> Tickets { get; set; }

    public DbSet<PartModel> Parts { get; set; }

    public DbSet<TaskModel> Tasks { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<TicketModel>()
            .ToTable("Tickets");

        builder.Entity<PartModel>()
            .ToTable("Parts");

        builder.Entity<TaskModel>()
            .ToTable("Tasks");

        builder.Entity<ApplicationUser>()
            .ToTable("AspNetUsers");
        
    }
}
