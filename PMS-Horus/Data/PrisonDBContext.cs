using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using PMS_Horus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PMS_Horus.Data
{
    public class PrisonDBContext : DbContext
    {
        public DbSet<Prisoner> Prisoners { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<BehaviorRecord> BehaviorRecords { get; set; }

        public PrisonDBContext(DbContextOptions<PrisonDBContext> dbContextOptions) : base(dbContextOptions) { }

        public PrisonDBContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=MSI\SQLEXPRESS;Database=HorusDB;Trusted_Connection=True;TrustServerCertificate=True;");
                optionsBuilder.EnableSensitiveDataLogging(false);
                optionsBuilder.EnableDetailedErrors(false);

                optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuted));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Направи PersonalIDNumber уникален (задължително!)
            modelBuilder.Entity<Prisoner>()
                .HasIndex(p => p.PersonalIDNumber)
                .IsUnique();

            // 2. Настройка на 1:1 за MedicalRecord (ползваме системното Id)
            modelBuilder.Entity<Prisoner>()
                .HasOne(p => p.MedicalRecord)
                .WithOne(m => m.Prisoner)
                .HasForeignKey<MedicalRecord>(m => m.PrisonerId); // Създай колона PrisonerId в MedicalRecord

            // 3. Настройка на 1:Много за BehaviorRecord (ползваме системното Id)
            modelBuilder.Entity<Prisoner>()
                .HasMany(p => p.BehaviorRecords)
                .WithOne(b => b.Prisoner)
                .HasForeignKey(b => b.PrisonerId); // Създай колона PrisonerId в BehaviorRecord
        }


    }
}
