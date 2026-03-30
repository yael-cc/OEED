using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Models;
using Microsoft.AspNetCore.Http;
using OEED_ITT.Helpers;
using iText.Commons.Actions;

namespace OEED_ITT.Controllers
{
    public class LoginController : Controller
    {
        private readonly EventosInstitucionalesContext _context;
        //private readonly UsuarioG usuarioG;

        public LoginController(EventosInstitucionalesContext context)
        {
            _context = context;
            //usuarioG = new UsuarioG();
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: LoginController/Details/5
        public async Task<IActionResult> SetUsuarioG(int idUsuario, string contrasenaUsuario)
        {
            if (idUsuario == null||contrasenaUsuario == null)
            {
                UsuarioG.mensajeError = "La contraseña o el usuario no son validos.";
                return RedirectToAction("Index", "Login");
            }
            var usuario = await _context.Usuarios
                .Include(u => u.IddepartamentoNavigation)
                .Include(u => u.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == idUsuario && m.ContrasenaUsuario == contrasenaUsuario);

            if (usuario == null) {
                UsuarioG.mensajeError = "El usuario no existe.";
                return RedirectToAction("Index", "Login");
            }

            UsuarioG.IdUsuario = usuario.IdUsuario;
            UsuarioG.NombreUsuario = usuario.NombreUsuario;
            UsuarioG.ApellidoUsuario = usuario.ApellidoUsuario;
            UsuarioG.NombrePfUsuario = usuario.NombrePfUsuario;
            UsuarioG.CorreoUsuario = usuario.CorreoUsuario;
            UsuarioG.Idrol = usuario.Idrol;
            UsuarioG.Iddepartamento = usuario.Iddepartamento;
            UsuarioG.NombreDepartamento = usuario.IddepartamentoNavigation.Nombre;
            UsuarioG.ContrasenaUsuario = usuario.ContrasenaUsuario;

            if (usuario.Idrol == 3) {
                return RedirectToAction("HomeIndex", "Evento");
            }
            if (usuario.Idrol == 2)
            {
                return RedirectToAction("HomeIndex", "EventoO");
            }
            if (usuario.Idrol == 1)
            {
                return RedirectToAction("HomeIndex", "EventoA");
            }
            UsuarioG.mensajeError = "Ocurrio un error inesperado.";
            return RedirectToAction("Index", "Login");
        }

        // GET: LoginController/Create

        public ActionResult PerfilUsuario()
        {
            return View();
        }
        public ActionResult PerfilUsuarioA()
        {
            return View();
        }

        public ActionResult PerfilUsuarioO()
        {
            return View();
        }

        public ActionResult CambiarPassword() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambioPassword(int noCtrl, string oldP, string newP) { 
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == noCtrl && u.ContrasenaUsuario == oldP);

            if (usuario == null) {
                UsuarioG.mensajeError = "No se ha encontrado el usuario";
                return RedirectToAction("CambiarPassword", "Login");
            }

            usuario.ContrasenaUsuario = newP;

            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                _context.SaveChangesAsync();
                UsuarioG.mensajeError = "Se ha cambiado con exito la contraseña";
                return RedirectToAction("CambiarPassword", "Login");
            }

            UsuarioG.mensajeError = "Se ha producido un error inesperado.";
            return RedirectToAction("CambiarPassword", "Login");
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
