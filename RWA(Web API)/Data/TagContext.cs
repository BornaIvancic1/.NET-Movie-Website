using Microsoft.EntityFrameworkCore;
using RWA_Web_API_.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RWA_Web_API_.Data
{
    public class TagContext : DbContext
    {
        public DbSet<Tag> Tag { get; set; }
        

        public TagContext(DbContextOptions<TagContext> options) : base(options)
        {
        }

    }
}
