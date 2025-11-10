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

        // 📋 LISTA DE CITAS MÉDICAS
        [HttpGet]
        public IActionResult ListaCitasMedicas(string? dni)
        {
            var citas = dbContext.Citas_Medicas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dni))
                citas = citas.Where(c => c.Paciente.DNI_Paciente.ToString().Contains(dni));

            return View(citas
                .OrderByDescending(c => c.Fecha_CitaMedica)
                .ThenByDescending(c => c.Hora_CitaMedica)
                .ToList());
        }

        // 🆕 REGISTRAR CITA - GET
        [HttpGet]
        public IActionResult RegistrarCitaPaciente()
        {
            return View();
        }

        [HttpPost]
public IActionResult RegistrarCitaPaciente(CitaMedica cita)
{
    if (!ModelState.IsValid)
        return View(cita);

    var medico = dbContext.Medicos.Find(cita.MedicoId);
    if (medico == null)
    {
        ModelState.AddModelError("MedicoId", "Debe seleccionar un médico válido.");
        return View(cita);
    }

    // Día de la semana (en español)
    string diaSemana = cita.Fecha_CitaMedica.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));
    diaSemana = char.ToUpper(diaSemana[0]) + diaSemana.Substring(1); // "Lunes", "Martes", etc.

    // Buscar horario del médico para ese día
    var horario = dbContext.Horarios_Medicos
        .FirstOrDefault(h => h.MedicoId == cita.MedicoId && h.DiaSemana == diaSemana);

    if (horario == null)
    {
        ModelState.AddModelError("", $"El médico {medico.Nombre_Completo_Medico} no atiende los días {diaSemana}.");
        return View(cita);
    }

    // Validar que la hora elegida esté dentro del rango del horario
    if (cita.Hora_CitaMedica < horario.Hora_Inicio || cita.Hora_CitaMedica >= horario.Hora_Fin)
    {
        ModelState.AddModelError("", $"El médico atiende los {diaSemana}s entre {horario.Hora_Inicio} y {horario.Hora_Fin}.");
        return View(cita);
    }

    // Validar si ya hay una cita en el mismo bloque de 30 minutos
    bool ocupado = dbContext.Citas_Medicas.Any(c =>
        c.MedicoId == cita.MedicoId &&
        c.Fecha_CitaMedica.Date == cita.Fecha_CitaMedica.Date &&
        c.Hora_CitaMedica == cita.Hora_CitaMedica);

    if (ocupado)
    {
        ModelState.AddModelError("", $"El médico ya tiene una cita a las {cita.Hora_CitaMedica:hh\\:mm} en esa fecha.");
        return View(cita);
    }

    // Guardar si todo está correcto
    dbContext.Citas_Medicas.Add(cita);
    dbContext.SaveChanges();

    return RedirectToAction("ListaCitasMedicas");
}


        // ✏️ EDITAR CITA - GET
        [HttpGet]
        public IActionResult EditarCitaPaciente(int id)
        {
            var cita = dbContext.Citas_Medicas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefault(c => c.Id_CitaMedica == id);

            if (cita == null)
                return NotFound();

            ViewBag.Medicos = dbContext.Medicos.ToList();
            return View(cita);
        }

        // ✏️ EDITAR CITA - POST
        [HttpPost]
        public IActionResult EditarCitaPaciente(CitaMedica cita)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Medicos = dbContext.Medicos.ToList();
                return View(cita);
            }

            // Verificar que el nuevo horario siga siendo válido
            var horario = dbContext.Horarios_Medicos
                .AsEnumerable()
                .FirstOrDefault(h =>
                    h.MedicoId == cita.MedicoId &&
                    h.DiaSemana.Equals(cita.Fecha_CitaMedica.ToString("dddd", new System.Globalization.CultureInfo("es-ES")),
                        StringComparison.OrdinalIgnoreCase) &&
                    cita.Hora_CitaMedica >= h.Hora_Inicio &&
                    cita.Hora_CitaMedica < h.Hora_Fin);

            if (horario == null)
            {
                ModelState.AddModelError("", "El nuevo horario no coincide con la disponibilidad del médico.");
                ViewBag.Medicos = dbContext.Medicos.ToList();
                return View(cita);
            }

            dbContext.Citas_Medicas.Update(cita);
            dbContext.SaveChanges();
            return RedirectToAction("ListaCitasMedicas");
        }

        // ❌ ELIMINAR CITA - GET
        [HttpGet]
        public IActionResult EliminarCitaPaciente(int id)
        {
            var cita = dbContext.Citas_Medicas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .FirstOrDefault(c => c.Id_CitaMedica == id);

            if (cita == null)
                return NotFound();

            return View(cita);
        }

        // ❌ ELIMINAR CITA - POST
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
