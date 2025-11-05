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

        // 📋 LISTAR HORARIOS
        public IActionResult ListaHorariosMedicos()
        {
            var horarios = dbContext.Horarios_Medicos
                .Include(h => h.Medico)
                .OrderBy(h => h.Medico.Nombre_Completo_Medico)
                .ThenBy(h => h.DiaSemana)
                .ToList();

            return View(horarios);
        }

        // 🆕 REGISTRAR HORARIO - GET
        [HttpGet]
        public IActionResult RegistrarHorarioMedico(int? medicoId)
        {
            ViewBag.Medicos = new SelectList(dbContext.Medicos.ToList(), "Id_Medico", "Nombre_Completo_Medico", medicoId);
            return View();
        }

        // 🆕 REGISTRAR HORARIO - POST
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

            // ✅ Redirigir al detalle del médico
            return RedirectToAction("DetalleMedico", "Medicos", new { id = horario.MedicoId });
        }


        // ✏️ EDITAR HORARIO - GET
        [HttpGet]
        public IActionResult EditarHorarioMedico(int id)
        {
            var horario = dbContext.Horarios_Medicos.Find(id);
            if (horario == null)
                return NotFound();

            ViewBag.Medicos = new SelectList(dbContext.Medicos.ToList(), "Id_Medico", "Nombre_Completo_Medico", horario.MedicoId);
            return View(horario);
        }

        // ✏️ EDITAR HORARIO - POST
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

        // ❌ ELIMINAR HORARIO - GET (Confirmación)
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
        public IActionResult EliminarHorarioMedicoConfirmado(HorarioMedico horario)
        {
            var horarioDb = dbContext.Horarios_Medicos.Find(horario.Id_Horario);
            if (horarioDb != null)
            {
                dbContext.Horarios_Medicos.Remove(horarioDb);
                dbContext.SaveChanges();
            }

            return RedirectToAction("DetalleMedico", "Medicos", new { id = horarioDb.MedicoId });

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
                .AsEnumerable() // 👈 Esto hace que el resto del filtro se ejecute en memoria
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
