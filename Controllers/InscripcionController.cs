using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Models;
using OEED_ITT.Helpers;

namespace OEED_ITT.Controllers
{
    public class InscripcionController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public InscripcionController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: Inscripcion
        public async Task<IActionResult> Index()
        {
            var eventosInstitucionalesContext = _context.Inscripcions
            .Where(i => i.IdUsuario == UsuarioG.IdUsuario)
            .Include(i => i.OEvento).Where(i => i.OEvento.IdEstadoEvento == 1)
            .Include(i => i.oEstadoI)
            .Include(i => i.oUsuario);
            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        // GET: Inscripcion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcions
                .Include(i => i.OEvento)
                .Include(i => i.oEstadoI)
                .Include(i => i.oUsuario)
                .FirstOrDefaultAsync(m => m.IdInscripcion == id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        //GET: Inscripcion/Create
        public IActionResult Create(int idEvento)
        {
            // Obtener el evento específico por su Id
            var evento = _context.Eventos.FirstOrDefault(e => e.IdEvento == idEvento);
            if (evento == null)
            {
                return NotFound(); // O manejar el error de alguna otra manera
            }

            // Configurar los datos necesarios para la vista
            ViewData["IdEvento"] = idEvento; // Solo necesitas el Id del evento
            ViewData["IdUsuario"] = UsuarioG.IdUsuario; // IdUsuario por defecto

            return View();
        }


        // POST: Inscripcion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(int idEvento, int idUsuario)
        {
            var inscripcionExistente = _context.Inscripcions.FirstOrDefault(i => i.IdEvento == idEvento && i.IdUsuario == idUsuario);
            if (inscripcionExistente != null)
            {
                inscripcionExistente.IdEstadoInscripcion = 1;
                inscripcionExistente.HoraInscripcion = DateTime.Now;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", "Inscripcion");
            }

            Inscripcion inscripcion = new Inscripcion
            {
                IdEvento = idEvento,
                HoraInscripcion = DateTime.Now,
                IdEstadoInscripcion = 1,
                IdUsuario = idUsuario
            };

            if (ModelState.IsValid)
            {
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Inscripcion");
            }

            return View(inscripcion);
        }



        // GET: Inscripcion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcions.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", inscripcion.IdEvento);
            ViewData["IdEstadoInscripcion"] = new SelectList(_context.EstadoInscripcions, "IdEstadoInscripcion", "IdEstadoInscripcion", inscripcion.IdEstadoInscripcion);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", inscripcion.IdUsuario);
            return View(inscripcion);
        }

        // POST: Inscripcion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInscripcion,HoraInscripcion,IdUsuario,IdEvento,IdEstadoInscripcion")] Inscripcion inscripcion)
        {
            if (id != inscripcion.IdInscripcion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcionExists(inscripcion.IdInscripcion))
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
            ViewData["IdEvento"] = new SelectList(_context.Eventos, "IdEvento", "IdEvento", inscripcion.IdEvento);
            ViewData["IdEstadoInscripcion"] = new SelectList(_context.EstadoInscripcions, "IdEstadoInscripcion", "IdEstadoInscripcion", inscripcion.IdEstadoInscripcion);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", inscripcion.IdUsuario);
            return View(inscripcion);
        }

        public async Task<IActionResult> Cancelar(int idEvento, int idUsuario)
        {
            var eventoUsuario = await _context.Inscripcions
                .FirstOrDefaultAsync(eu => eu.IdEvento == idEvento && eu.IdUsuario == idUsuario);

            if (eventoUsuario == null)
            {
                return NotFound(); // Registro no encontrado
            }

            // Cambiar el valor de la columna requerida
            eventoUsuario.IdEstadoInscripcion = 2;
            eventoUsuario.HoraInscripcion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction("Index", "Inscripcion");
        }


        // GET: Inscripcion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcions
                .Include(i => i.OEvento)
                .Include(i => i.oEstadoI)
                .Include(i => i.oUsuario)
                .FirstOrDefaultAsync(m => m.IdInscripcion == id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // POST: Inscripcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcion = await _context.Inscripcions.FindAsync(id);
            if (inscripcion != null)
            {
                _context.Inscripcions.Remove(inscripcion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripcions.Any(e => e.IdInscripcion == id);
        }
    }
}
