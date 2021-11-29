using System;
using Microsoft.EntityFrameworkCore;
using App.Domain;
using Microsoft.Extensions.Logging;

namespace App.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        private StreamWriter _writer = new StreamWriter("../EFCoreLog.txt", append: true);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // tell ef core to use sqlserver and pass the corresponding connectionstring
            optionsBuilder
                .UseSqlServer("Server=localhost,1433;Initial Catalog=SamuraiAppData;User ID=sa;Password=Test123!")
                // Logs to console
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                // Log to a file
                // .LogTo(_writer.WriteLine);
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ----------------------
            // get reference to samurai model
            modelBuilder.Entity<Samurai>()
            // get battles property stored in samurai (samurai has many battles)
            .HasMany(s => s.Battles)
            // get samurais of battle class (battle with many samurais)
            .WithMany(b => b.Samurais)
            // ------------------ the block marked with the lines tells ef core that we are talking about this many-to-many relationship
            // next line: use the BattleSamurai class to infert the relationship table instead of infering the table by yourself
            // 1:n relation ship with battle and 1:n relationship with samurai
            .UsingEntity<BattleSamurai>(bs => bs.HasOne<Battle>().WithMany(), bs => bs.HasOne<Samurai>().WithMany())
            // next two lines: tell ef core to infer the DateJoined column in the BattleSamurai table with default getdate
            .Property(bs => bs.DateJoined)
            .HasDefaultValueSql("getdate()");
        }
    }
}