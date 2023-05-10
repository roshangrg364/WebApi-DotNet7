using CoreModule.Mapping;
using CoreModule.Src;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.DbContextConfig
{
    public class ApplicationDbContext:IdentityDbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IConfiguration configuration):base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new VillaMapping());
            modelBuilder.ApplyConfiguration(new VillaNumberMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("VillaApi"));
            }
        }
    }
}
