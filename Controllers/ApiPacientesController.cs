using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers   
{

    [ApiController]
    [Route("api/pacientes")]
    public class ApiPacientesController : ControllerBase  // ControllerBase es la clase base para controladores que no necesitan vistas.
    {
        private readonly ApplicationDbContext dbContext;

        public ApiPacientesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)  // su ruta seria http://localhost:5177/api/pacientes/login
        {
            var paciente = dbContext.Pacientes.FirstOrDefault(p => p.Correo_Paciente == req.Correo && p.Contrasenia_Paciente == req.Contrasenia);


            if (paciente == null)
            {
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos" });
            }


            return Ok(new
            {
                id = paciente.Id_Paciente,
                nombre = paciente.Nombre_Completo_Paciente,
                dni = paciente.DNI_Paciente,
                telefono = paciente.Telefono_Paciente,
                fechaNaciemiento = paciente.Fecha_Nacimiento_Paciente,
                direccion = paciente.Direccion_Paciente,
                correo = paciente.Correo_Paciente
            });
        }




        [HttpGet("{id}/citas")]
        public IActionResult GetCitas(int id)  // su ruta seria http://localhost:5177/api/pacientes/1/citas 
        {
            var citas = dbContext.Citas_Medicas
                .Include(c => c.Medico)
                .ThenInclude(m => m.Especialidad)
                .Where(c => c.PacienteId == id)
                .OrderByDescending(c => c.Fecha_CitaMedica)
                .Select(c => new CitaMedicaDTO
                {
                    Id = c.Id_CitaMedica,
                    Especialidad = c.Medico.Especialidad.Nombre_Especialidad,
                    Medico = c.Medico.Nombre_Completo_Medico,
                    Fecha = c.Fecha_CitaMedica.ToString("yyyy-MM-dd"),
                    Hora = c.Hora_CitaMedica.ToString(@"hh\:mm"),
                    Estado = c.Estado_CitaMedica
                })
                .ToList();

            return Ok(citas);
        }




    }



    public class LoginRequest
    {
        public string Correo { get; set; }

        public string Contrasenia { get; set; }
    }


}