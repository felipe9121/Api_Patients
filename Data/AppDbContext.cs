using API_LM.Models;
using Microsoft.EntityFrameworkCore;

namespace API_LM.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientFieldsSP>()
                .HasNoKey()
                .ToView(null);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.DocumentNumber)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}