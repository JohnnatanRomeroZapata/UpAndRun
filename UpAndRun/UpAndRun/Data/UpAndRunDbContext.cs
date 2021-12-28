using Microsoft.EntityFrameworkCore;
using UpAndRun.Models;

namespace UpAndRun.Data
{
    public class UpAndRunDbContext : DbContext
    {

        public UpAndRunDbContext(DbContextOptions<UpAndRunDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplicationType> ApplicationTypes { get; set; }

        public DbSet<Product> Products { get; set; }

    }
}
