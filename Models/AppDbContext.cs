using Microsoft.EntityFrameworkCore;

namespace SISTEMARH_BACKEND.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Unidade> Unidades { get; set; }
    }
}
