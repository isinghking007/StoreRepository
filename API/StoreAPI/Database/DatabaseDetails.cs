using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;

namespace StoreAPI.Database
{
    public class DatabaseDetails:DbContext
    {
        public DatabaseDetails(DbContextOptions<DatabaseDetails> options):base(options) { }

        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CompanyDetails> CompanyDetails { get; set; }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<ProductDetails> ProductDetails { get; set; }
        public DbSet<DueDetails> DueDetails { get; set; }
        public DbSet<BorrowerDetails> BorrowerDetails { get; set; }
        public DbSet<FormData> FormData { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
                .HasIndex(e => new {e.MobileNumber,e.UserName,e.CompanyId})
                .IsUnique();
        }
    }
}
