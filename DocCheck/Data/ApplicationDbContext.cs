using DocCheck.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DocCheck.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<CheckType> CheckTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentCheck> DocumentChecks { get; set; }
        public DbSet<DocumentRejection> DocumentRejections { get; set; }
        public DbSet<DocumentStatus> DocumentStatuses { get; set; }
        public DbSet<RejectionReason> RejectionReasons { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CheckType>().HasKey(e => e.Id);

            builder.Entity<Document>().HasKey(e => e.Id);
            builder.Entity<Document>().HasOne(e => e.Status).WithMany(e => e.Documents)
                .HasForeignKey(e => e.StatusId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DocumentCheck>().HasKey(e => e.Id);
            builder.Entity<DocumentCheck>().HasOne(e => e.Document).WithMany(e => e.Checks)
                .HasForeignKey(e => e.DocumentId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<DocumentCheck>().HasOne(e => e.CheckedBy).WithMany()
                .HasForeignKey(e => e.CheckedById).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DocumentCheck>().HasOne(e => e.CheckType).WithMany(e => e.Checks)
                .HasForeignKey(e => e.CheckTypeId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DocumentRejection>().HasKey(e => e.Id);
            builder.Entity<DocumentRejection>().HasOne(e => e.Document).WithMany(e => e.Rejects)
                .HasForeignKey(e => e.DocumentId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<DocumentRejection>().HasOne(e => e.RejectedBy).WithMany()
                .HasForeignKey(e => e.RejectedById).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<DocumentRejection>().HasOne(e => e.RejectionReason).WithMany(e => e.Rejects)
                .HasForeignKey(e => e.RejectionReasonId).HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DocumentStatus>().HasKey(e => e.Id);

            builder.Entity<RejectionReason>().HasKey(e => e.Id);
        }
    }
}
