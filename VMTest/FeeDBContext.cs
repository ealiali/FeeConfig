using Microsoft.EntityFrameworkCore;

namespace VMTest
{
    public class FeeDBContext:DbContext
    {
        public DbSet<FeeConfiguration> FeeConfigurations { get; set; }
        public FeeDBContext(DbContextOptions options):base(options)
        {
        }
    }
}
