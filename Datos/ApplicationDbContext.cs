using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;
namespace TrabajoFinalGrupo6DBP
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Paciente> Pacientes { get; set; }

        public DbSet<CitaMedica> Citas_Medicas { get; set; }
        
    }
}