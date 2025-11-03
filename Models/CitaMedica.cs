using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrabajoFinalGrupo6DBP.Models
{
    public class CitaMedica
    {
        [Key]
        public int Id_CitaMedica { get; set; }

        [Required]
        [ForeignKey("Paciente")]
        public int PacienteId { get; set; } // Llave foránea hacia Paciente

        public Paciente? Paciente { get; set; } // Esto es para que podamos navegar desde CitaMedica hacia Paciente

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha_CitaMedica { get; set; }

        [Required]
        public TimeSpan Hora_CitaMedica { get; set; }

        [Required]
        public string Especialidad { get; set; }

    
        public string? Medico { get; set; }

        public string? Observaciones { get; set; }

        [Required]
        public string Estado_CitaMedica { get; set; } = "En proceso"; // Confirmada, Cancelada, etc.
    }
}
