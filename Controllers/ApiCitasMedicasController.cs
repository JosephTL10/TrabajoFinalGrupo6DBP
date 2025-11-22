using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP;
using TrabajoFinalGrupo6DBP.Models;

[ApiController]
[Route("api/citas")]
public class ApiCitasController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    public ApiCitasController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    [HttpPut("{id}/cancelar")]
    public IActionResult CancelarCita(int id)  // su ruta seria http://localhost:5177/api/citas/1/cancelar
    {
        var cita = dbContext.Citas_Medicas.Find(id);

        if (cita == null)
            return NotFound(new { 
                mensaje = "Cita no encontrada" 
            });

        cita.Estado_CitaMedica = "Cancelada";
        dbContext.SaveChanges();

        return Ok(new { mensaje = "Cita cancelada correctamente" });
    }
}
