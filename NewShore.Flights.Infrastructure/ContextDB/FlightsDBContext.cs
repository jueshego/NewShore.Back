using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NewShore.Flights.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewShore.Flights.Infrastructure.ContextDB
{
    public class FlightsDBContext : DbContext
    {
        public FlightsDBContext()
        {
        }

        public FlightsDBContext(DbContextOptions<FlightsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Transport> Transport { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }

        public virtual DbSet<Journey> Flights { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder().
                                SetBasePath(Directory.GetCurrentDirectory()).
                                AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            string conexString = config.GetValue<string>("ConnectionStrings:flightsSql");

            optionsBuilder.UseSqlite(conexString);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
