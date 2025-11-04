using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class CitasMedicasController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CitasMedicasController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult ListaCitasMedicas(string? dni)
        {
            var citas = dbContext.Citas_Medicas
                .Include(c => c.Paciente)
                .AsQueryable();
            if (!string.IsNullOrEmpty(dni))
            {
                citas = citas.Where(c => c.Paciente.DNI_Paciente.ToString().Contains(dni));
            }

            return View(citas.OrderByDescending(c => c.Fecha_CitaMedica).ToList());
        }

        // REGISTRAR
        [HttpGet]
        public IActionResult RegistrarCitaPaciente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarCitaPaciente(CitaMedica cita)
        {
            if (!ModelState.IsValid)
            {
                return View(cita);
            }
            dbContext.Citas_Medicas.Add(cita);
            dbContext.SaveChanges();

            return RedirectToAction("ListaCitasMedicas");
        }

        // EDITAR
        [HttpGet]
        public IActionResult EditarCitaPaciente(int id)
        {
            var cita = dbContext.Citas_Medicas.Find(id);
            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        [HttpPost]
        public IActionResult EditarCitaPaciente(CitaMedica cita)
        {
            if (!ModelState.IsValid)
            {
                return View(cita);
            }
            dbContext.Citas_Medicas.Update(cita);
            dbContext.SaveChanges();
            return RedirectToAction("ListaCitasMedicas");
        }

        [HttpGet]
        public IActionResult EliminarCitaPaciente(int id)
        {
            var cita = dbContext.Citas_Medicas
                .Include(c => c.Paciente)
                .FirstOrDefault(c => c.Id_CitaMedica == id);

            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        [HttpPost]
        public IActionResult EliminarCitaPacienteConfirm(int id)
        {
            var cita = dbContext.Citas_Medicas.Find(id);
            if (cita != null)
            {
                dbContext.Citas_Medicas.Remove(cita);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ListaCitasMedicas");
        }
    }
}
