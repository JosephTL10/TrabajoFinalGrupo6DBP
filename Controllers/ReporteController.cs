using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabajoFinalGrupo6DBP.Controllers;
namespace TrabajoFinalGrupo6DBP.Controllers
{
    public class ReporteController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ReporteController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GenerarReporte()
        {
            return View();
        }

        
        
    }
}