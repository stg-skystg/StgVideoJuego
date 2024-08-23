using Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAcces
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<VideoGame> VideoGame { get; set; }
        public DbSet<Calificaciones> Calificaciones { get; set; }
    }
}
