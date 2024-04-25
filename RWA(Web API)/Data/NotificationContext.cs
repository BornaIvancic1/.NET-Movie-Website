using Microsoft.EntityFrameworkCore;
using RWA_Web_API_.Models;

namespace RWA_Web_API_.Data
{
    public class NotificationContext:DbContext
    {
        public DbSet<Notification> Notification { get; set; }


        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }

    }
}
