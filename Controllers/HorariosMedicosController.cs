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

        // ❌ ELIMINAR HORARIO - POST
        [HttpPost]
        public IActionResult EliminarHorarioMedicoConfirmado(int id)
        {
            var horario = dbContext.Horarios_Medicos.Find(id);
            if (horario != null)
            {
                dbContext.Horarios_Medicos.Remove(horario);
                dbContext.SaveChanges();
            }

            return RedirectToAction("ListaHorariosMedicos");
        }
    }
}
