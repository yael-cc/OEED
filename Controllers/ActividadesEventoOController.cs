using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Helpers;
using OEED_ITT.Models;

namespace OEED_ITT.Controllers
{
    public class ActividadesEventoOController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public ActividadesEventoOController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int idEvento)
        {
            var eventosInstitucionalesContext = _context.ActividadesEventos.Include(a => a.IdEventoNavigation).Where(a => a.IdEvento == idEvento);
            ViewData["IdEvento"] = idEvento;
            if (EventoNoIniciado(idEvento))
            {
                ViewData["CondicionEvento"] = true;
            }
            else
            {
                ViewData["CondicionEvento"] = false;
            }
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
        public IActionResult Create(int idEvento)
        {
            ViewData["IdEvento"] = idEvento;
            return View();
        }

        // POST: ActividadesEvento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string nombreActividad,string descripcionActividad,DateTime horaInicio,DateTime horaFin,int idEvento)
        {
            var evento = _context.Eventos.
                        Where(e => e.IdEvento == idEvento)
                        .FirstOrDefault();

            ActividadesEvento actividadesEvento = new ActividadesEvento
            {
                NombreActividad = nombreActividad,
                DescripcionActividad = descripcionActividad,
                HoraInicio = horaInicio,
                HoraFin = horaFin,
                IdEvento = idEvento
            };

            if (horaInicio < evento.HoraInicioEvento || horaInicio > evento.HoraFinEvento || horaFin < evento.HoraInicioEvento || horaFin > evento.HoraFinEvento)
            {
                UsuarioG.mensajeError = "La hora de inicio debe ser mayor o igual a la hora de inicio del evento y menor a la hora de fin del evento," +
                    " asi como la hora de fin debe ser mayor a la hora de inicio del evento y menor o igual que la hora del fin del evento";
                ViewData["IdEvento"] = idEvento;
                return View(actividadesEvento);
            }
            else if (horaFin < horaInicio) {
                UsuarioG.mensajeError = "La hora de inicio debe ser menor que la hora del final";
                ViewData["IdEvento"] = idEvento;
                return View(actividadesEvento);
            }

            if (ModelState.IsValid)
            {
                _context.Add(actividadesEvento);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ActividadesEventoO", new { idEvento = idEvento });
            }

            ViewData["IdEvento"] = idEvento;
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
        public async Task<IActionResult> Borrar(int id)
        {
            var actividadesEvento = await _context.ActividadesEventos.FindAsync(id);
            if (actividadesEvento != null)
            {
                _context.ActividadesEventos.Remove(actividadesEvento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ActividadesEventoO", new { idEvento = id});
        }

        private bool ActividadesEventoExists(int id)
        {
            return _context.ActividadesEventos.Any(e => e.IdActividad == id);
        }

        private bool EventoNoIniciado(int idEvento)
        {
            return _context.Eventos.Any(e => e.IdEvento == idEvento && e.IdEstadoEvento == 1);
        }

    }
}
