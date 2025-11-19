using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PacientesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        
        public IActionResult ListaPacientes()
        {
            var listaPacientes = dbContext.Pacientes.ToList();
            return View(listaPacientes);
        }

        // Para buscar pacientes por DNI
        [HttpGet]
        public IActionResult ListaPacientes(string? dni)
        {
            var pacientes = dbContext.Pacientes.AsQueryable();

            if (!string.IsNullOrEmpty(dni))
            {
                pacientes = pacientes.Where(p => p.DNI_Paciente.ToString().Contains(dni));
            }

            return View(pacientes.ToList());
        }



        [HttpGet]
        public IActionResult RegistrarPaciente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarPaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var existeDNI = dbContext.Pacientes.Any(p => p.DNI_Paciente == paciente.DNI_Paciente);
                if (existeDNI)
                {
                    ModelState.AddModelError("DNI_Paciente", "El DNI ya está registrado.");
                    return View(paciente);
                }
                
                dbContext.Pacientes.Add(paciente);
                dbContext.SaveChanges();
                return RedirectToAction("ListaPacientes");
            }
            return View(paciente);
        }



        [HttpGet]
        public IActionResult EditarRegistroPaciente(int id)
        {
            var paciente = dbContext.Pacientes.Find(id);
            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        [HttpPost]
        public IActionResult EditarRegistroPaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                dbContext.Pacientes.Update(paciente);
                dbContext.SaveChanges();
                return RedirectToAction("ListaPacientes");
            }
            return View(paciente);
        }


        [HttpGet]
        public IActionResult EliminarRegistroPaciente(int id)
        {
            var paciente = dbContext.Pacientes.FirstOrDefault(p => p.Id_Paciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente); // retorna la vista de confirmación con los detalles del paciente
        }

        [HttpPost]
        public IActionResult EliminarRegistroPacienteConfirm(int id)
        {
            var paciente = dbContext.Pacientes.FirstOrDefault(p => p.Id_Paciente == id);
            if (paciente != null)
            {
                dbContext.Pacientes.Remove(paciente);
                dbContext.SaveChanges();
                return RedirectToAction("ListaPacientes");
            }

            return NotFound();
        }




        [HttpGet]
        public IActionResult DetallePaciente(int id)
        {
            var paciente = dbContext.Pacientes
            .Include(p => p.CitasMedicas)
            .ThenInclude(c => c.Medico)
            .ThenInclude(m => m.Especialidad)
            .FirstOrDefault(p => p.Id_Paciente == id);
            
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }


        
        [HttpGet]
        public IActionResult BuscarPorDNI(int dni)
        {
            var paciente = dbContext.Pacientes.FirstOrDefault(p => p.DNI_Paciente == dni);
            if (paciente == null)
            {
                return Json(null);
            }

            // Calcular edad del paciente
            int edad = DateTime.Now.Year - paciente.Fecha_Nacimiento_Paciente.Year;
            if (DateTime.Now.DayOfYear < paciente.Fecha_Nacimiento_Paciente.DayOfYear)
                edad--;

            return Json(new
            {
                id = paciente.Id_Paciente,
                nombreCompleto = $"{paciente.Nombre_Completo_Paciente}",
                edad = edad
            });
        }
    }
}
