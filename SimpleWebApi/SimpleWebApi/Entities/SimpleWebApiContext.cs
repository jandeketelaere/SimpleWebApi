using Microsoft.EntityFrameworkCore;

namespace SimpleWebApi.Entities
{
    public class SimpleWebApiContext : DbContext
    {
        public DbSet<Apple> Apples { get; set; }

        public SimpleWebApiContext(DbContextOptions<SimpleWebApiContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
        }
    }
}