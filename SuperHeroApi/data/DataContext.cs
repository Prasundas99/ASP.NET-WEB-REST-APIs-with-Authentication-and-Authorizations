using Microsoft.EntityFrameworkCore;

namespace SuperHeroApi.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SuperHeros> SuperHeros { get; set; }
    }
}