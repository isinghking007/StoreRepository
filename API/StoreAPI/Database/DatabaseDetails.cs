using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;

namespace StoreAPI.Database
{
    public class DatabaseDetails:DbContext
    {
        public DatabaseDetails(DbContextOptions<DatabaseDetails> options):base(options) { }

        public DbSet<UserInfo> Users { get; set; }
    }
}
