using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class MedicosController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public MedicosController(ApplicationDbContext dbcontext)
        {
            this.dbContext = dbcontext;
        }

        
        public IActionResult ListaMedicos()
        {
            var medicos = dbContext.Medicos
                .OrderBy(m => m.Nombre_Completo_Medico)
                .ToList();

            return View(medicos);
        }

        
        [HttpGet]
        public IActionResult RegistrarMedico()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult RegistrarMedico(Medico medico)
        {
            if (!ModelState.IsValid)
                return View(medico);

            // Validar duplicados por DNI
            bool existe = dbContext.Medicos.Any(m => m.DNI_Medico == medico.DNI_Medico);
            if (existe)
            {
                ModelState.AddModelError("DNI_Medico", "Ya existe un médico registrado con este DNI.");
                return View(medico);
            }

            dbContext.Medicos.Add(medico);
            dbContext.SaveChanges();

            return RedirectToAction("DetalleMedico", new { id = medico.Id_Medico });

        }

        
        [HttpGet]
        public IActionResult EditarMedico(int id)
        {
            var medico = dbContext.Medicos.Find(id);
            if (medico == null)
                return NotFound();

            return View(medico);
        }

        
        [HttpPost]
        public IActionResult EditarMedico(Medico medico)
        {
            if (!ModelState.IsValid)
                return View(medico);

            dbContext.Medicos.Update(medico);
            dbContext.SaveChanges();

            return RedirectToAction("ListaMedicos");
        }

        
        [HttpGet]
        public IActionResult EliminarMedico(int id)
        {
            var medico = dbContext.Medicos.Find(id);
            if (medico == null)
                return NotFound();

            return View(medico);
        }



        
        [HttpPost]
        public IActionResult EliminarMedicoConfirmado(int id)
        {
            var medico = dbContext.Medicos
                .Include(m => m.Horarios)
                .Include(m => m.CitasMedicas)
                .FirstOrDefault(m => m.Id_Medico == id);

            if (medico == null)
                return NotFound();

            if ((medico.Horarios != null && medico.Horarios.Any()) ||
                (medico.CitasMedicas != null && medico.CitasMedicas.Any()))
            {
                ViewBag.Error = "❌ No se puede eliminar este médico porque tiene horarios o citas asignadas. "
                        + "Elimínelos o reasígnelos antes de continuar.";
                return View("EliminarMedico", medico);
            }

            dbContext.Medicos.Remove(medico);
            dbContext.SaveChanges();

            return RedirectToAction("ListaMedicos");
        }




        
        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var medico = dbContext.Medicos.Find(id);
            if (medico != null)
            {
                medico.Estado_Medico = !medico.Estado_Medico;
                dbContext.SaveChanges();
            }

            return RedirectToAction("ListaMedicos");
        }





        
        public IActionResult DetalleMedico(int id)
        {
            
            var medico = dbContext.Medicos
                .Include(m => m.Horarios)
                .FirstOrDefault(m => m.Id_Medico == id);

            if (medico == null)
                return NotFound();

            return View(medico);
        }

    }
}
