using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalGrupo6DBP.Models
{
    public class Especialidad
    {
        [Key]
        public int Id_Especialidad { get; set; }

        [Required]
        public string Nombre_Especialidad { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        
        public bool Estado_Especialidad { get; set; } = true; // Indica si la especialidad está activa

        public ICollection<Medico>? Medicos { get; set; } // Una especialidad puede tener muchos médicos
    }
}