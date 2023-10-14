using BharatMirror.Models;
using Microsoft.EntityFrameworkCore;

namespace BharatMirror.Data
{
    public class AppDbContext : DbContext
    {
        public IConfiguration Configuration { get; }


        public AppDbContext(IConfiguration conn)
        {
            Configuration = conn;
        }
        public DbSet<Users> Users { get; set; }

        public DbSet<Advertisement> Advertisement { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? conString = Configuration.GetConnectionString("BharatmirrorDbConnection");




            optionsBuilder.UseSqlServer(@conString);
        }

      
    }


}
