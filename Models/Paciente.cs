using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalGrupo6DBP.Models
{
    public class Paciente
    {
        [Key]
        public int Id_Paciente { get; set; }

        [Required]
        public int DNI_Paciente { get; set; }

        [Required]
        public string Nombre_Completo_Paciente { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento_Paciente { get; set; }

        [Required]
        public string Telefono_Paciente { get; set; }

        [Required]
        public string Direccion_Paciente { get; set; }

        // Relación con Cita Médica
        public ICollection<CitaMedica>? CitasMedicas { get; set; } // Esto es para que un paciente pueda tener múltiples citas médicas
    }
}
