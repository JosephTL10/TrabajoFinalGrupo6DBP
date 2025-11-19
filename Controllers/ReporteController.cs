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
            ViewBag.Especialidades = dbContext.Especialidades
                .Where(e => e.Estado_Especialidad)
                .ToList();

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
            ViewBag.Especialidades = dbContext.Especialidades
                .Where(e => e.Estado_Especialidad)
                .ToList();

            // ======================================
            // ==========   PACIENTES   =============
            // ======================================
            if (tipoReporte == "pacientes")
            {
                var pacientes = dbContext.Pacientes.AsQueryable();

                if (filtroPaciente == "porNombre" && !string.IsNullOrEmpty(nombrePacienteFiltro))
                    pacientes = pacientes.Where(p => p.Nombre_Completo_Paciente.Contains(nombrePacienteFiltro));

                ViewBag.TipoReporte = "pacientes";
                ViewBag.Resultados = pacientes.ToList();

                return View();
            }

            // ======================================
            // ==========     CITAS     =============
            // ======================================
            else if (tipoReporte == "citas")
            {
                var citas = dbContext.Citas_Medicas
                    .Include(c => c.Paciente)
                    .Include(c => c.Medico)
                    .ThenInclude(m => m.Especialidad)
                    .AsQueryable();

                if (filtroCita == "fecha" && fechaInicio.HasValue && fechaFin.HasValue)
                    citas = citas.Where(c => c.Fecha_CitaMedica >= fechaInicio && c.Fecha_CitaMedica <= fechaFin);

                if (filtroCita == "especialidad" && !string.IsNullOrEmpty(especialidad))
                    citas = citas.Where(c => c.Medico.Especialidad.Nombre_Especialidad == especialidad);

                if (filtroCita == "estado" && !string.IsNullOrEmpty(estado))
                    citas = citas.Where(c => c.Estado_CitaMedica == estado);

                if (filtroCita == "paciente" && !string.IsNullOrEmpty(nombrePaciente))
                    citas = citas.Where(c => c.Paciente.Nombre_Completo_Paciente.Contains(nombrePaciente));

                ViewBag.TipoReporte = "citas";
                ViewBag.Resultados = citas.ToList();

                return View();
            }

            // ======================================
            // ========   ESTADISTICAS   ============
            // ======================================
            else if (tipoReporte == "estadisticas")
            {
                var citas = dbContext.Citas_Medicas
                    .Include(c => c.Medico)
                    .ThenInclude(m => m.Especialidad)
                    .AsQueryable();

                if (tipoEstadistica == "porEspecialidad")
                {
                    var estadisticas = citas
                        .GroupBy(c => c.Medico.Especialidad.Nombre_Especialidad)
                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                        .ToList();

                    ViewBag.Estadisticas = estadisticas;



                    ViewBag.Labels = estadisticas.Select(e => e.Categoria).ToList();
                    ViewBag.Data = estadisticas.Select(e => e.Total).ToList();
                }

                else if (tipoEstadistica == "porEstado")
                {
                    var estadisticas = citas
                        .GroupBy(c => c.Estado_CitaMedica)
                        .Select(g => new { Categoria = g.Key, Total = g.Count() })
                        .ToList();

                    ViewBag.Estadisticas = estadisticas;



                    ViewBag.Labels = estadisticas.Select(e => e.Categoria).ToList();
                    ViewBag.Data = estadisticas.Select(e => e.Total).ToList();
                }

                else if (tipoEstadistica == "porMes")
                {
                    var estadisticas = citas
                        .GroupBy(c => c.Fecha_CitaMedica.Month)
                        .Select(g => new {
                            Categoria = 
                                System.Globalization.CultureInfo
                                .GetCultureInfo("es-ES")
                                .DateTimeFormat.GetMonthName(g.Key),
                            Total = g.Count()
                        })
                        .ToList();

                    ViewBag.Estadisticas = estadisticas;


                    ViewBag.Labels = estadisticas.Select(e => e.Categoria).ToList();
                    ViewBag.Data = estadisticas.Select(e => e.Total).ToList();
                }

                ViewBag.TipoReporte = "estadisticas";
                return View();
            }

            ViewBag.Mensaje = "Seleccione un tipo de reporte válido.";
            return View();
        }


        
    }
}
