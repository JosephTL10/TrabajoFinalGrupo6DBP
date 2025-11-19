using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("Especialidad")]
        public int Id_Especialidad { get; set; } // Foreign key to Especialidad
        public Especialidad? Especialidad { get; set; } // Navigation property


        [Required]
        public string Telefono_Medico { get; set; }

        [EmailAddress]
        public string Correo_Medico { get; set; }


        public bool Estado_Medico { get; set; } = true;

        public ICollection<CitaMedica>? CitasMedicas { get; set; } // Un médico puede tener muchas citas médicas


        public ICollection<HorarioMedico>? Horarios { get; set; } // Un médico puede tener muchos horarios

    }
}
