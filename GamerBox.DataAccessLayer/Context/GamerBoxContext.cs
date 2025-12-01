using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace GamerBox.DataAccessLayer.Context
{
    public class GamerBoxContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SQL Server bağlantı 
            optionsBuilder.UseSqlServer(
                "Server=EREN\\SQLEXPRESS;Database=GamerBoxDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İlişkiler, kısıtlamalar vb. burada tanımlanabilir (şimdilik boş )
            base.OnModelCreating(modelBuilder);
        }
    }
}
