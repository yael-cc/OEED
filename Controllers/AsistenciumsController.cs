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
    public class AsistenciumsController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public AsistenciumsController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: Asistenciums
        public async Task<IActionResult> Index(int idEvento)
        {
            var eventosInstitucionalesContext = _context.Asistencia.Include(a => a.IdEventoNavigation).Include(a => a.IdUsuarioNavigation).Include(a => a.IdUsuarioNavigation.IddepartamentoNavigation).Where(a => a.IdEvento == idEvento);
            ViewData["IdEvento"] = idEvento;
            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        // GET: Asistenciums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencium = await _context.Asistencia
                .Include(a => a.IdEventoNavigation)
                .Include(a => a.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdAsistencia == id);
            if (asistencium == null)
            {
                return NotFound();
            }

            return View(asistencium);
        }

        // GET: Asistenciums/Create
        public IActionResult Create()
        {
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Asistenciums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsistencia,IdEvento,IdUsuario,HoraAsistencia,DetallesAsistencia")] Asistencium asistencium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistencium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", asistencium.IdEvento);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", asistencium.IdUsuario);
            return View(asistencium);
        }

        public async Task<IActionResult> CreatePost(int idEvento, int idUsuario)
        {
            var asistenciaExistente = _context.Asistencia.FirstOrDefault(i => i.IdEvento == idEvento && i.IdUsuario == idUsuario);
            if (asistenciaExistente != null)
            {
                UsuarioG.mensajeError = "El usuario ya esta registrado en la lista de asistencia";
                return RedirectToAction("Index", "Asistenciums", new { idEvento = idEvento });
            }

            var usuarioInexistente = _context.Usuarios.FirstOrDefault(i => i.IdUsuario == idUsuario);
            if (usuarioInexistente == null)
            {
                UsuarioG.mensajeError = "El usuario no existe o no esta registrado en el sistema";
                return RedirectToAction("Index", "Asistenciums", new { idEvento = idEvento });
            }

            Asistencium asistencia = new Asistencium
            {
                IdEvento = idEvento,
                IdUsuario = idUsuario,
                HoraAsistencia = DateTime.Now,
                DetallesAsistencia = null
            };

            if (ModelState.IsValid)
            {
                _context.Add(asistencia);
                await _context.SaveChangesAsync();
                UsuarioG.mensajeError = "";
                return RedirectToAction("Index", "Asistenciums", new { idEvento = idEvento });
            }

            UsuarioG.mensajeError = "Se presento un error inesperado";
            return RedirectToAction("Index", "Asistenciums", new { idEvento = idEvento });
        }

        // GET: Asistenciums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencium = await _context.Asistencia.FindAsync(id);
            if (asistencium == null)
            {
                return NotFound();
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", asistencium.IdEvento);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", asistencium.IdUsuario);
            return View(asistencium);
        }

        // POST: Asistenciums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsistencia,IdEvento,IdUsuario,HoraAsistencia,DetallesAsistencia")] Asistencium asistencium)
        {
            if (id != asistencium.IdAsistencia)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistencium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciumExists(asistencium.IdAsistencia))
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
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", asistencium.IdEvento);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", asistencium.IdUsuario);
            return View(asistencium);
        }

        // GET: Asistenciums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistencium = await _context.Asistencia
                .Include(a => a.IdEventoNavigation)
                .Include(a => a.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdAsistencia == id);
            if (asistencium == null)
            {
                return NotFound();
            }

            return View(asistencium);
        }

        // POST: Asistenciums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asistencium = await _context.Asistencia.FindAsync(id);
            if (asistencium != null)
            {
                _context.Asistencia.Remove(asistencium);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAsistencia(int idEvento, int idUsuario)
        {
            var asistencia = await _context.Asistencia.FirstOrDefaultAsync(i => i.IdEvento == idEvento && i.IdUsuario == idUsuario);

            if (asistencia == null)
            {
                TempData["ErrorMessage"] = "No se encontró la asistencia marcada para eliminar.";
                return RedirectToAction("Index", "Asistenciums", new { idEvento = idEvento });
            }

            // Eliminar la entidad encontrada
            _context.Asistencia.Remove(asistencia);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Redirigir a la página principal o a otra página según sea necesario
            return RedirectToAction("Index", "Asistenciums",new {idEvento = idEvento});
        }

        private bool AsistenciumExists(int id)
        {
            return _context.Asistencia.Any(e => e.IdAsistencia == id);
        }
    }
}
