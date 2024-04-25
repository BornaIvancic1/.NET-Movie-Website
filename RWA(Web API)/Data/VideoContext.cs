using Microsoft.EntityFrameworkCore;
using RWA_Web_API_.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RWA_Web_API_.Data
{
    public class VideoContext:DbContext
    {
        public DbSet<Video> Video { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Image> Image { get; set; }

        public VideoContext(DbContextOptions<VideoContext> options) : base(options)
        {
        }
       
       
    }
}
