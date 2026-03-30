using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Helpers;
using OEED_ITT.Models;

namespace OEED_ITT.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public UsuarioController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var eventosInstitucionalesContext = _context.Usuarios.Include(u => u.IddepartamentoNavigation).Include(u => u.IdrolNavigation);
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            ViewData["Idrol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View(await eventosInstitucionalesContext.ToListAsync());
        }
        
        public async Task<IActionResult> IndexF(string? idDepartamento, int? idRol)
        {
            var eventosInstitucionalesContext = _context.Usuarios.Include(u => u.IddepartamentoNavigation).Include(u => u.IdrolNavigation);
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            ViewData["Idrol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            if (idDepartamento != null && idRol == null)
            {
                var eventosInstitucionalesContext2 = eventosInstitucionalesContext.Where(i => i.Iddepartamento == idDepartamento);
                return View(await eventosInstitucionalesContext2.ToListAsync());
            }else if (idDepartamento == null && idRol != null)
            {
                var eventosInstitucionalesContext2 = eventosInstitucionalesContext.Where(i => i.Idrol == idRol);
                return View(await eventosInstitucionalesContext2.ToListAsync());
            }else if (idDepartamento != null && idRol != null)
            {
                var eventosInstitucionalesContext2 = eventosInstitucionalesContext.Where(i => i.Idrol == idRol && i.Iddepartamento == idDepartamento);
                return View(await eventosInstitucionalesContext2.ToListAsync());
            }

            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                UsuarioG.mensajeError = "Inserte un numero de control.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IddepartamentoNavigation)
                .Include(u => u.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                UsuarioG.mensajeError = "El usuario no existe, intentelo de nuevo.";
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            ViewData["Idrol"] = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,NombreUsuario,ApellidoUsuario,Idrol,Iddepartamento,NombrePfUsuario,CorreoUsuario,ContrasenaUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", usuario.Iddepartamento);
            ViewData["Idrol"] = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.Idrol);
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", usuario.Iddepartamento);
            ViewData["Idrol"] = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.Idrol);
            ViewData["ContrasenaUsuario"] = usuario.ContrasenaUsuario;
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario,ApellidoUsuario,Idrol,Iddepartamento,NombrePfUsuario,CorreoUsuario,ContrasenaUsuario")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", usuario.Iddepartamento);
            ViewData["Idrol"] = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.Idrol);
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IddepartamentoNavigation)
                .Include(u => u.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
