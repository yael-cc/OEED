using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OEED_ITT.Helpers;
using OEED_ITT.Models;
using System.Diagnostics;

namespace OEED_ITT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventosInstitucionalesContext _context;

        public HomeController(ILogger<HomeController> logger, EventosInstitucionalesContext context)
        {
            _logger = logger;
            _context = context; // Asigna el contexto de la base de datos a la propiedad privada
        }

        public IActionResult Index()
        {
            //Numero de control del estudiante
            ViewData["IdUsuario"] = UsuarioG.IdUsuario;
            //
            ViewData["NombreCompleto"] = (UsuarioG.ApellidoUsuario + " "+ UsuarioG.NombreUsuario + "");
            ViewData["NombreDepartamento"] = UsuarioG.NombreDepartamento;
            
            DateTime now = DateTime.Now;
            var eventoInscripcion = (from evento in _context.Eventos
                                     join inscripcion in _context.Inscripcions
                                     on evento.IdEvento equals inscripcion.IdEvento
                                     where inscripcion.IdUsuario == UsuarioG.IdUsuario &&
                                           evento.IdEstadoEvento == 1 &&
                                           evento.HoraFinEvento > now
                                     select evento)
                        .FirstOrDefault();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
