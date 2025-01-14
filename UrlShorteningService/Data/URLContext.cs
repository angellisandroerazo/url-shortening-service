using Microsoft.EntityFrameworkCore;
using UrlShorteningService.Models;

namespace UrlShorteningService.Data
{
    public class URLContext : DbContext
    {
        public URLContext(DbContextOptions<URLContext> options)
        : base(options)
        {
            
        }

        public DbSet<URL> URL { get; set; }
    }
}