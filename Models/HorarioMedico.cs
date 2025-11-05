using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabajoFinalGrupo6DBP.Models
{
    public class HorarioMedico
    {
        [Key]
        public int Id_Horario { get; set; }

        [Required]
        [ForeignKey("Medico")]
        public int MedicoId { get; set; }

        public Medico? Medico { get; set; }

        [Required]
        public string DiaSemana { get; set; } // Ejemplo: "Lunes", "Martes", etc.

        [Required]
        public TimeSpan Hora_Inicio { get; set; } // Ejemplo: 08:00

        [Required]
        public TimeSpan Hora_Fin { get; set; }    // Ejemplo: 14:00
    }
}
