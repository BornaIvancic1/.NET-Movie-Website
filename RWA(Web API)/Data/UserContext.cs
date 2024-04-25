using Microsoft.EntityFrameworkCore;
using RWA_Web_API_.Models;

namespace RWA_Web_API_.Data
{
    public class UserContext:DbContext
    {
        public DbSet<User> User { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
    }
}
