using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PMS_Horus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;


namespace PMS_Horus.Data
{
    public class PrisonDBContext : DbContext
    {
        public DbSet<Prisoner> Prisoners { get; set; }

        public PrisonDBContext(DbContextOptions<PrisonDBContext> dbContextOptions) : base(dbContextOptions) { }

        public PrisonDBContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=MSI\SQLEXPRESS;Database=HorusDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
