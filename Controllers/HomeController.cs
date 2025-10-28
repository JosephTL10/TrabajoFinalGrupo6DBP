using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrabajoFinalGrupo6DBP.Models;

namespace TrabajoFinalGrupo6DBP.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    private readonly ApplicationDbContext dbContext;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        dbContext = context;
    }

    

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Inicio()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpGet]
    public IActionResult Registro()
    {
        
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Registro(Usuario usuario)
    {
        if (ModelState.IsValid)
            {
                // Evita correos duplicados y DNIs duplicados
                if (dbContext.Usuarios.Any(u => u.Correo_Electronico_Usuario == usuario.Correo_Electronico_Usuario || u.DNI_Usuario == usuario.DNI_Usuario))
                {
                    ViewBag.Mensaje = "El correo o DNI ya están registrados.";
                    return View();
                }

                dbContext.Usuarios.Add(usuario);
                dbContext.SaveChanges();

                ViewBag.Mensaje = "Registro exitoso. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            return View();
    }


    [HttpGet]
    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(Usuario usuario)
    {
        
        var usuarioParaEncontrar = dbContext.Usuarios.FirstOrDefault(u => u.Correo_Electronico_Usuario == usuario.Correo_Electronico_Usuario && u.Contrasenia_Usuario == usuario.Contrasenia_Usuario);

        if (usuarioParaEncontrar == null)
        {
            ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña incorrectos.");
            return View();
        }

        return RedirectToAction("Inicio");
        
    }


    [HttpGet]
    public IActionResult RecuperarContrasenia()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RecuperarContrasenia(string Correo_Electronico_Usuario)
    {
        
        var usuario = dbContext.Usuarios.FirstOrDefault(u => u.Correo_Electronico_Usuario == Correo_Electronico_Usuario);
        if (usuario != null)
        {
            TempData["CorreoRecuperacion"] = Correo_Electronico_Usuario;
            return RedirectToAction("CambiarContrasenia");
        }
        return View();
    
        
    }



    [HttpGet]
    public IActionResult CambiarContrasenia()
    {
    
        if (TempData["CorreoRecuperacion"] == null)
        {
            return RedirectToAction("RecuperarContrasenia");
        }

        // Se conserva el correo para el POST siguiente
        ViewBag.Correo = TempData["CorreoRecuperacion"];
        TempData.Keep("CorreoRecuperacion");
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CambiarContrasenia(string Contrasenia_Usuario)
    {
        var correo = TempData["CorreoRecuperacion"] as string;

        if (string.IsNullOrEmpty(correo))
        {
            return RedirectToAction("RecuperarContrasenia");
        }

        var usuario = dbContext.Usuarios.FirstOrDefault(u => u.Correo_Electronico_Usuario == correo);

        if (usuario != null)
        {
            usuario.Contrasenia_Usuario = Contrasenia_Usuario;
            dbContext.SaveChanges();

            TempData.Remove("CorreoRecuperacion");
            return RedirectToAction("Login");
        }

        ModelState.AddModelError(string.Empty, "Error al cambiar la contraseña.");
        
        return View();

    }
    







    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
