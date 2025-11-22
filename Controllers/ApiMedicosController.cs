using Microsoft.AspNetCore.Mvc;
using TrabajoFinalGrupo6DBP;

namespace TrabajoFinalGrupo6DBP.Controllers
{
    [Route("api/medicos")] 
    [ApiController]
    public class ApiMedicosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ApiMedicosController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("porEspecialidad")]
        public IActionResult GetMedicosPorEspecialidad(int especialidadId)   // http://localhost:5177/api/medicos/porEspecialidad?especialidadid=1  ejemplo
        {
            var medicos = dbContext.Medicos
                .Where(m => m.Id_Especialidad == especialidadId && m.Estado_Medico)
                .Select(m => new
                {
                    id = m.Id_Medico,
                    nombre = m.Nombre_Completo_Medico
                })
                .ToList();

            return Ok(medicos);
        }
    }
}
