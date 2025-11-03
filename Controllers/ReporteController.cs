using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class ReporteController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ReporteController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public IActionResult GenerarReporte()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerarReporte(
            string tipoReporte,
            string? filtroCita,
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? especialidad,
            string? estado,
            string? nombrePaciente,
            string? filtroPaciente,
            string? nombrePacienteFiltro,
            string? tipoEstadistica)
        {
            if (tipoReporte == "pacientes")
            {
                var pacientes = dbContext.Pacientes.AsQueryable();

                if (filtroPaciente == "porNombre" && !string.IsNullOrEmpty(nombrePacienteFiltro))
                {
                    pacientes = pacientes.Where(p => p.Nombre_Completo_Paciente.Contains(nombrePacienteFiltro));
                }

                ViewBag.TipoReporte = "pacientes"; // Indicar el tipo de reporte
                ViewBag.Resultados = pacientes.ToList(); // Asignar resultados a ViewBag
                return View();
            }

            else if (tipoReporte == "citas")
            {
                var citas = dbContext.Citas_Medicas
                    .Include(c => c.Paciente)
                    .AsQueryable(); // Incluir Paciente para filtros    

                if (filtroCita == "fecha" && fechaInicio.HasValue && fechaFin.HasValue)
                {
                    citas = citas.Where(c => c.Fecha_CitaMedica >= fechaInicio && c.Fecha_CitaMedica <= fechaFin);
                }
                if (filtroCita == "especialidad" && !string.IsNullOrEmpty(especialidad))
                {
                    citas = citas.Where(c => c.Especialidad == especialidad);
                }
                if (filtroCita == "estado" && !string.IsNullOrEmpty(estado))
                {
                    citas = citas.Where(c => c.Estado_CitaMedica == estado);
                }
                if (filtroCita == "paciente" && !string.IsNullOrEmpty(nombrePaciente))
                {
                    citas = citas.Where(c => c.Paciente.Nombre_Completo_Paciente.Contains(nombrePaciente));
                }

                ViewBag.TipoReporte = "citas";
                ViewBag.Resultados = citas.ToList();
                return View();
            }

            else if (tipoReporte == "estadisticas")
            {
                var citas = dbContext.Citas_Medicas.AsQueryable(); // Incluir Paciente no es necesario para estadísticas

                if (tipoEstadistica == "porEspecialidad")
                {
                    var estadisticas = citas
                        .GroupBy(c => c.Especialidad)
                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                        .ToList(); 

                    ViewBag.TipoReporte = "estadisticas";
                    ViewBag.Estadisticas = estadisticas;
                }
                else if (tipoEstadistica == "porEstado")
                {
                    var estadisticas = citas
                        .GroupBy(c => c.Estado_CitaMedica)
                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                        .ToList();

                    ViewBag.TipoReporte = "estadisticas";
                    ViewBag.Estadisticas = estadisticas;
                }
                else if (tipoEstadistica == "porMes")
                {
                    var estadisticas = citas
                        .GroupBy(c => c.Fecha_CitaMedica.Month)
                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                        .ToList();

                    ViewBag.TipoReporte = "estadisticas";
                    ViewBag.Estadisticas = estadisticas;
                }

                return View();
            }

            ViewBag.Mensaje = "Seleccione un tipo de reporte válido.";
            return View();
        }
    }
}
