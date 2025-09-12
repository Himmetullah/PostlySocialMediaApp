using Microsoft.EntityFrameworkCore;
using Postly.WebAPI.Models;

namespace Postly.WebAPI.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
