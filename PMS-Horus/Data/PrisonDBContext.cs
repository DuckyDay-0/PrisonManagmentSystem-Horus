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
    }
}
