using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Models;

namespace OEED_ITT.Controllers
{
    public class ActividadesEventoController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public ActividadesEventoController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: ActividadesEvento
        public async Task<IActionResult> Index(int idEvento)
        {
            var eventosInstitucionalesContext = _context.ActividadesEventos.Include(a => a.IdEventoNavigation).Where(a => a.IdEvento == idEvento);
            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        // GET: ActividadesEvento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividadesEvento = await _context.ActividadesEventos
                .Include(a => a.IdEventoNavigation)
                .FirstOrDefaultAsync(m => m.IdActividad == id);
            if (actividadesEvento == null)
            {
                return NotFound();
            }

            return View(actividadesEvento);
        }

        // GET: ActividadesEvento/Create
        public IActionResult Create()
        {
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento");
            return View();
        }

        // POST: ActividadesEvento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdActividad,NombreActividad,DescripcionActividad,HoraInicio,HoraFin,IdEvento")] ActividadesEvento actividadesEvento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actividadesEvento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", actividadesEvento.IdEvento);
            return View(actividadesEvento);
        }

        // GET: ActividadesEvento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividadesEvento = await _context.ActividadesEventos.FindAsync(id);
            if (actividadesEvento == null)
            {
                return NotFound();
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", actividadesEvento.IdEvento);
            return View(actividadesEvento);
        }

        // POST: ActividadesEvento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdActividad,NombreActividad,DescripcionActividad,HoraInicio,HoraFin,IdEvento")] ActividadesEvento actividadesEvento)
        {
            if (id != actividadesEvento.IdActividad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actividadesEvento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActividadesEventoExists(actividadesEvento.IdActividad))
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
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", actividadesEvento.IdEvento);
            return View(actividadesEvento);
        }

        // GET: ActividadesEvento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actividadesEvento = await _context.ActividadesEventos
                .Include(a => a.IdEventoNavigation)
                .FirstOrDefaultAsync(m => m.IdActividad == id);
            if (actividadesEvento == null)
            {
                return NotFound();
            }

            return View(actividadesEvento);
        }

        // POST: ActividadesEvento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actividadesEvento = await _context.ActividadesEventos.FindAsync(id);
            if (actividadesEvento != null)
            {
                _context.ActividadesEventos.Remove(actividadesEvento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActividadesEventoExists(int id)
        {
            return _context.ActividadesEventos.Any(e => e.IdActividad == id);
        }
    }
}
