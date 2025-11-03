using System.ComponentModel.DataAnnotations;

namespace TrabajoFinalGrupo6DBP.Models
{
    public class Usuario
    {
        [Key]
        public int Id_Usuario { get; set; }

        [Required]
        public int DNI_Usuario { get; set; }

        [Required]
        public string Nombres_Usuario { get; set; }

        [Required]
        public string Apellidos_Usuario { get; set; }

        [Required]
        public string Telefono_Usuario { get; set; }

        [Required]
        [EmailAddress]
        public string Correo_Electronico_Usuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contrasenia_Usuario { get; set; }  

        
    }
}
