using JWTTestproj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace JWTTestproj
{
    public class JWTContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public JWTContext(DbContextOptions<JWTContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected  void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var decodeConnectionString = _configuration.GetSection("ConnectionStrings")["DefaultConnection"];
                optionsBuilder.UseSqlServer(decodeConnectionString);
            }
        }

        #region EntityTable
        public DbSet<User_Table> Users { get; set; }
        #endregion
    }
}
