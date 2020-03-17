using BattlestaHealthChecks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Sample> Sample { get; set; }
        public DbSet<CheckedElement> CheckedElement { get; set; }
        public DbSet<SampleSettings> SampleSettings { get; set; }
        public DbSet<BackupSettings> BackupSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>().HasKey(c => c.Id);
            modelBuilder.Entity<Sample>()
                .HasMany(s => s.Elements)
                .WithOne(e => e.Sample);

            modelBuilder.Entity<CheckedElement>().HasKey(k => k.Id);
            modelBuilder.Entity<CheckedElement>()
                .HasOne(el => el.Sample)
                .WithMany(sa => sa.Elements);
        }
    }
}
