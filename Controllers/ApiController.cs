using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ApiController(ApplicationDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }

        // 🔹 Médicos por especialidad
        [HttpGet("medicos/porEspecialidad")]
        public IActionResult GetMedicosPorEspecialidad(string especialidad)
        {
            var medicos = dbContext.Medicos
                .Where(m => m.Especialidad == especialidad && m.Estado_Medico)
                .Select(m => new { id = m.Id_Medico, nombre = m.Nombre_Completo_Medico })
                .ToList();

            return Ok(medicos);
        }

        // 🔹 Horarios por médico
        [HttpGet("horarios/porMedico")]
        public IActionResult GetHorariosPorMedico(int id)
        {
            var horarios = dbContext.Horarios_Medicos
                .Where(h => h.MedicoId == id)
                .Select(h => new
                {
                    dia = h.DiaSemana,
                    horaInicio = h.Hora_Inicio.ToString(@"hh\:mm"),
                    horaFin = h.Hora_Fin.ToString(@"hh\:mm")
                })
                .ToList();

            return Ok(horarios);
        }
    }
}
