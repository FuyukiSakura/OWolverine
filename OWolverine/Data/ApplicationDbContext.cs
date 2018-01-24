using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OWolverine.Models;
using OWolverine.Models.Ogame;

namespace OWolverine.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //Datasets
        public DbSet<Universe> Universes { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Alliance> Alliances { get; set; }
        public DbSet<Planet> Planets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Universe>().ToTable("Universe", "og");
            builder.ApplyConfiguration(new PlayerConfiguration());
            builder.ApplyConfiguration(new AllianceConfiguration());
            builder.ApplyConfiguration(new PlanetConfiguration());
            builder.Entity<Moon>().ToTable("Moon", "og");
            builder.ApplyConfiguration(new ScoreConfiguration());
            builder.Entity<ScoreHistory>().ToTable("ScoreHistory", "og");
        }
    }
}
