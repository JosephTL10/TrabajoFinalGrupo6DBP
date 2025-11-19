using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class EspecialidadesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EspecialidadesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        public IActionResult ListaEspecialidades()
        {
            var especialidades = dbContext.Especialidades
                .OrderBy(e => e.Nombre_Especialidad)
                .ToList();

            return View(especialidades);
        }

        


        
        [HttpGet]
        public IActionResult RegistrarEspecialidad()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult RegistrarEspecialidad(Especialidad especialidad)
        {
            if (!ModelState.IsValid)
                return View(especialidad);

            // Vvalidar que no se repita el nombre
            bool existe = dbContext.Especialidades
                .Any(e => e.Nombre_Especialidad.ToLower() == especialidad.Nombre_Especialidad.ToLower());

            if (existe)
            {
                ModelState.AddModelError("Nombre_Especialidad", "Ya existe una especialidad con este nombre.");
                return View(especialidad);
            }

            dbContext.Especialidades.Add(especialidad);
            dbContext.SaveChanges();

            return RedirectToAction("ListaEspecialidades");
        }



        
        [HttpGet]
        public IActionResult EditarEspecialidad(int id)
        {
            var especialidad = dbContext.Especialidades.Find(id);
            if (especialidad == null)
                return NotFound();

            return View(especialidad);
        }

        [HttpPost]
        public IActionResult EditarEspecialidad(int id, string descripcion)
        {
            var especialidad = dbContext.Especialidades.Find(id);
            if (especialidad == null)
                return NotFound();

            especialidad.Descripcion = descripcion;
            dbContext.SaveChanges();

            return RedirectToAction("ListaEspecialidades");
        }

        
        [HttpPost]
        public IActionResult CambiarEstado(int id)
        {
            var especialidad = dbContext.Especialidades.Find(id);
            if (especialidad == null)
                return NotFound();

            especialidad.Estado_Especialidad = !especialidad.Estado_Especialidad;
            dbContext.SaveChanges();

            return RedirectToAction("ListaEspecialidades");
        }



        
        [HttpGet]
        public IActionResult EliminarEspecialidad(int id)
        {
            var especialidad = dbContext.Especialidades
                .Include(e => e.Medicos)
                .FirstOrDefault(e => e.Id_Especialidad == id);

            if (especialidad == null)
                return NotFound();

            return View(especialidad);
        }

        [HttpPost]
        public IActionResult EliminarEspecialidadConfirmado(int id)
        {
            var especialidad = dbContext.Especialidades
                .Include(e => e.Medicos)
                .FirstOrDefault(e => e.Id_Especialidad == id);

            if (especialidad == null)
                return NotFound();

            if (especialidad.Medicos != null && especialidad.Medicos.Any())
            {
                TempData["Error"] = "❌ No se puede eliminar esta especialidad porque tiene médicos asignados.";
                return RedirectToAction("ListaEspecialidades");
            }

            dbContext.Especialidades.Remove(especialidad);
            dbContext.SaveChanges();

            return RedirectToAction("ListaEspecialidades");
        }
    }
}
