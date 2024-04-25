using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RWA_MVC____2.Models;

namespace RWA_MVC____2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<VideoTag> VideoTag { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<User> User { get; set; }
      
    }
}
