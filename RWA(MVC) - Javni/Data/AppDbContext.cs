using Microsoft.EntityFrameworkCore;
using RWA_MVC_JAVNI.Models;

namespace RWA_MVC_JAVNI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<User> User { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<VideoTag> VideoTag { get; set; }
        public DbSet<Video> Video { get; set; }
    }
}
