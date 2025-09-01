using DocCheck.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocCheck.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<SaleDoc> SaleDocs => Set<SaleDoc>();

        public DbSet<PaperworkError> PaperworkErrors => Set<PaperworkError>();

        public DbSet<QuantityError> QuantityErrors => Set<QuantityError>();

        public DbSet<SaleDocLog> SaleDocLogs => Set<SaleDocLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SaleDoc>().HasKey(e => e.Id);
            builder.Entity<SaleDoc>().HasOne(e => e.User).WithMany()
                .HasForeignKey(e => e.UserId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PaperworkError>().HasKey(e => e.Id);
            builder.Entity<PaperworkError>().HasOne(e => e.SaleDoc).WithMany(e => e.PaperworkErrors)
                .HasForeignKey(e => e.SaleDocId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuantityError>().HasKey(e => e.Id);
            builder.Entity<QuantityError>().HasOne(e => e.SaleDoc).WithMany(e => e.QuantityErrors)
                .HasForeignKey(e => e.SaleDocId).HasPrincipalKey(e => e.Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SaleDocLog>().HasKey(e => e.Id);
        }
    }
}
