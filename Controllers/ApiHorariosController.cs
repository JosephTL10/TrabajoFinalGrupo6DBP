using Microsoft.AspNetCore.Mvc;
using TrabajoFinalGrupo6DBP;


namespace TrabajoFinalGrupo6DBP.Controllers
{
    [Route("api/horarios")]
    [ApiController]

    public class ApiHorariosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ApiHorariosController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("porMedico")]
        public IActionResult GetHorariosPorMedico(int id)  // http://localhost:5177/api/horarios/porMedico?id=2  ejemplo
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

        [HttpGet("bloquesDisponibles")]
        public IActionResult GetBloquesDisponibles(int medicoId, DateTime fecha)  // http://localhost:5177/api/horarios/bloquesDisponibles?medicoId=2&fecha=2025-11-25  ejemplo
        {
            // Buscar el día en español (para comparar con el horario)
            string diaSemana = fecha.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
            diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1);

            // Buscar el horario del médico para ese día
            var horario = dbContext.Horarios_Medicos
            .FirstOrDefault(h => h.MedicoId == medicoId && h.DiaSemana == diaSemana);

            if (horario == null)
            {
                return Ok(new List<object>()); // No trabaja ese día
            }

            // Generar bloques de 30 minutos
            List<object> bloques = new List<object>();
            TimeSpan horaActual = horario.Hora_Inicio;

            while (horaActual < horario.Hora_Fin)
            {
                bool ocupado = dbContext.Citas_Medicas.Any(c =>
                    c.MedicoId == medicoId &&
                    c.Fecha_CitaMedica.Date == fecha.Date &&
                    c.Hora_CitaMedica == horaActual);

                bloques.Add(new
                {
                    hora = horaActual.ToString(@"hh\:mm"),
                    ocupado
                });

                horaActual = horaActual.Add(TimeSpan.FromMinutes(30));
            }

            return Ok(bloques);
        }
    }

}
