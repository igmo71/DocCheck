using DocCheck.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<DocumentCheck> DocumentCheck { get; set; }
        public DbSet<DocumentCheckLog> DocumentCheckLog { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DocumentCheck>().HasKey(e => e.Id);

            builder.Entity<DocumentCheck>().HasOne(e => e.User).WithMany()
                .HasForeignKey(e => e.UserId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DocumentCheckLog>().HasKey(e => e.Id);
        }
    }
}
