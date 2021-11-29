using Microsoft.EntityFrameworkCore;
using App.Domain;

namespace App.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // tell ef core to use sqlserver and pass the corresponding connectionstring
            optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=SamuraiAppData;User ID=sa;Password=Test123!");
        }
    }
}