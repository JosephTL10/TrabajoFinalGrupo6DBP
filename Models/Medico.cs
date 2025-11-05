using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalGrupo6DBP.Models
{
    public class Medico
    {
        [Key]
        public int Id_Medico { get; set; }

        [Required]
        public int DNI_Medico { get; set; } 

        [Required]
        public string Nombre_Completo_Medico { get; set; }

        [Required]
        public string Especialidad { get; set; } // Ejemplo: Cardiología, Dermatología, etc.

        [Required]
        public string Telefono_Medico { get; set; }

        [EmailAddress]
        public string Correo_Medico { get; set; }


        public bool Estado_Medico { get; set; } = true;

        public ICollection<CitaMedica>? CitasMedicas { get; set; }


        public ICollection<HorarioMedico>? Horarios { get; set; }

    }
}
