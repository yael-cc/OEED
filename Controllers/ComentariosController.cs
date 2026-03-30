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
    public class ComentariosController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public ComentariosController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index(int idEvento)
        {
            var eventosInstitucionalesContext = _context.Comentarios.Include(c => c.IdEventoNavigation).Include(c => c.IdUsuarioNavigation).Where(c => c.IdEvento == idEvento && c.IdUsuario == UsuarioG.IdUsuario);
            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.IdEventoNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdComentario == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // GET: Comentarios/Create
        public IActionResult Create(int idEvento)
        {
            ViewData["IdEvento"] = idEvento;
            return View();
        }

        public IActionResult CreateS(int idEvento)
        {
            ViewData["IdEvento"] = idEvento;
            return View();
        }

        // POST: Comentarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComentario,AsuntoComentario,DescripcionComentario,FechaComentario,IdUsuario,IdEvento")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comentario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", comentario.IdEvento);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", comentario.IdUsuario);
            return View(comentario);
        }

        public async Task<IActionResult> Comentar(int idEvento, string asuntoComentario, string descripcionComentario)
        {
            Comentario comentario = new Comentario
            {
                AsuntoComentario = asuntoComentario,
                DescripcionComentario = descripcionComentario,
                FechaComentario = DateTime.Now,
                IdUsuario = UsuarioG.IdUsuario,
                IdEvento = idEvento
            };

            if (ModelState.IsValid)
            {
                _context.Add(comentario);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexEF", "Evento");
            }

            ViewData["IdEvento"] = idEvento;
            return RedirectToAction("IndexEF", "Evento");
        }


        public async Task<IActionResult> ComentarStaff(int idEvento, string asuntoComentario, string descripcionComentario)
        {
            Comentario comentario = new Comentario
            {
                AsuntoComentario = "[STAFF] " +asuntoComentario,
                DescripcionComentario = descripcionComentario,
                FechaComentario = DateTime.Now,
                IdUsuario = UsuarioG.IdUsuario,
                IdEvento = idEvento
            };

            if (ModelState.IsValid)
            {
                _context.Add(comentario);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexFin", "Evento", new { idUsuario = UsuarioG.IdUsuario });
            }

            ViewData["IdEvento"] = idEvento;
            return RedirectToAction("IndexFin", "Evento", new { idUsuario = UsuarioG.IdUsuario});
        }


        // GET: Comentarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", comentario.IdEvento);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", comentario.IdUsuario);
            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComentario,AsuntoComentario,DescripcionComentario,FechaComentario,IdUsuario,IdEvento")] Comentario comentario)
        {
            if (id != comentario.IdComentario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comentario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComentarioExists(comentario.IdComentario))
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
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", comentario.IdEvento);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", comentario.IdUsuario);
            return View(comentario);
        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentarios
                .Include(c => c.IdEventoNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdComentario == id);
            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario != null)
            {
                _context.Comentarios.Remove(comentario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentarios.Any(e => e.IdComentario == id);
        }
    }
}
