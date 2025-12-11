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
        public GamerBoxContext(DbContextOptions<GamerBoxContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
        .HasOne(p => p.User)
        .WithMany(u => u.Posts)
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.Restrict);

          
     
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GamerBoxContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
