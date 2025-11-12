using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class HorariosMedicosController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HorariosMedicosController(ApplicationDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }

        // LISTAR HORARIOS
        public IActionResult ListaHorariosMedicos()
        {
            var horarios = dbContext.Horarios_Medicos
                .Include(h => h.Medico)  // Incluir datos del médico
                .OrderBy(h => h.Medico.Nombre_Completo_Medico) // Ordenar por nombre del médico
                .ThenBy(h => h.DiaSemana) // Luego por día de la semana
                .ToList(); // Ejecutar la consulta y obtener la lista

            return View(horarios); 
        }

        
        [HttpGet]
        public IActionResult RegistrarHorarioMedico(int? medicoId)
        {
            ViewBag.Medicos = new SelectList(dbContext.Medicos.ToList(), "Id_Medico", "Nombre_Completo_Medico", medicoId);
            return View();
        }

        
        [HttpPost]
        public IActionResult RegistrarHorarioMedico(HorarioMedico horario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Medicos = new SelectList(dbContext.Medicos.ToList(), "Id_Medico", "Nombre_Completo_Medico", horario.MedicoId);
                return View(horario);
            }

            dbContext.Horarios_Medicos.Add(horario);
            dbContext.SaveChanges();

            
            return RedirectToAction("DetalleMedico", "Medicos", new { id = horario.MedicoId });
        }


        
        [HttpGet]
        public IActionResult EditarHorarioMedico(int id)
        {
            var horario = dbContext.Horarios_Medicos.Find(id);
            if (horario == null)
                return NotFound();

            ViewBag.Medicos = new SelectList(dbContext.Medicos.ToList(), "Id_Medico", "Nombre_Completo_Medico", horario.MedicoId);
            return View(horario);
        }

        
        [HttpPost]
        public IActionResult EditarHorarioMedico(HorarioMedico horario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Medicos = new SelectList(dbContext.Medicos.ToList(), "Id_Medico", "Nombre_Completo_Medico", horario.MedicoId);
                return View(horario);
            }

            dbContext.Horarios_Medicos.Update(horario);
            dbContext.SaveChanges();
            return RedirectToAction("ListaHorariosMedicos");
        }


        [HttpGet]
        public IActionResult EliminarHorarioMedico(int id)
        {
            var horario = dbContext.Horarios_Medicos
                .Include(h => h.Medico)
                .FirstOrDefault(h => h.Id_Horario == id);

            if (horario == null)
                return NotFound();

            return View(horario);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarHorarioMedicoConfirmado(int id)
        {
            var horario = dbContext.Horarios_Medicos
                .Include(h => h.Medico)
                .FirstOrDefault(h => h.Id_Horario == id);

            if (horario == null)
                return NotFound();

            // Verificar si hay citas dentro de este horario
            var citasAsociadas = dbContext.Citas_Medicas
                .AsEnumerable() // ✅ necesario para usar ToString("dddd")
                .Any(c =>
                    {
                        string diaCita = c.Fecha_CitaMedica.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
                        return c.MedicoId == horario.MedicoId &&
                            diaCita.Equals(horario.DiaSemana, StringComparison.OrdinalIgnoreCase) &&
                            c.Hora_CitaMedica >= horario.Hora_Inicio &&
                            c.Hora_CitaMedica <= horario.Hora_Fin;
                    });

            if (citasAsociadas)
            {
                TempData["Error"] = "No se puede eliminar este horario porque tiene citas médicas programadas.";
                return RedirectToAction("ListaHorariosMedicos");
            }

            dbContext.Horarios_Medicos.Remove(horario);
            dbContext.SaveChanges();

            TempData["Exito"] = "Horario eliminado correctamente.";
            return RedirectToAction("ListaHorariosMedicos");
        }





        [HttpGet]
        public IActionResult VerCitasDeHorarioMedico(int id)
        {
            var horario = dbContext.Horarios_Medicos
                .Include(h => h.Medico)
                .FirstOrDefault(h => h.Id_Horario == id);

            if (horario == null)
                return NotFound();

            // Traemos las citas del médico y luego filtramos en memoria
            var citas = dbContext.Citas_Medicas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.MedicoId == horario.MedicoId)
                .AsEnumerable() // Esto hace que el resto del filtro se ejecute en memoria
                .Where(c =>
                    {
                        string diaCita = c.Fecha_CitaMedica.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
                            return diaCita.Equals(horario.DiaSemana, StringComparison.OrdinalIgnoreCase)
                                && c.Hora_CitaMedica >= horario.Hora_Inicio
                                && c.Hora_CitaMedica <= horario.Hora_Fin;
                    })
                    .OrderBy(c => c.Fecha_CitaMedica)
                    .ThenBy(c => c.Hora_CitaMedica)
                    .ToList();

            ViewBag.Horario = horario;
            return View(citas);
        }



    }
}
