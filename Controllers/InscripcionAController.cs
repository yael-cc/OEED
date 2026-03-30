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
    public class InscripcionAController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public InscripcionAController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: Inscripcion
        public async Task<IActionResult> Index()
        {
            var eventosInstitucionalesContext = _context.Inscripcions
            .Include(i => i.OEvento)
            .Include(i => i.oEstadoI)
            .Include(i => i.oUsuario)
            .Where(i => i.IdUsuario == UsuarioG.IdUsuario && i.OEvento.IdEstadoEvento == 1);
            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        public async Task<IActionResult> IndexGO(int? id)
        {
            var eventosInstitucionalesContext = _context.GrupoOrganizadors
            .Include(i => i.IdUsuarioNavigation)
            .Include(i => i.IdUsuarioNavigation.IddepartamentoNavigation)
            .Include(i => i.IdUsuarioNavigation.IdrolNavigation)
            .Where(i => i.IdEvento == id);

            ViewData["idEvento"] = id;

            return View(await eventosInstitucionalesContext.ToListAsync());

        }

        public async Task<IActionResult> IndexGS(int? id)
        {
            var eventosInstitucionalesContext = _context.GrupoOrganizadors
            .Include(i => i.IdUsuarioNavigation)
            .Include(i => i.IdUsuarioNavigation.IddepartamentoNavigation)
            .Include(i => i.IdUsuarioNavigation.IdrolNavigation)
            .Where(i => i.IdEvento == id);

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
                return RedirectToAction("Index", "EventoA");
            }

            Inscripcion inscripcion = new Inscripcion
            {
                IdEvento = idEvento,
                HoraInscripcion = DateTime.Now,
                IdEstadoInscripcion = 2,
                IdUsuario = idUsuario
            };

            if (ModelState.IsValid)
            {
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "EventoA");
            }

            return View(inscripcion);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGO(int idEvento, int idUsuario)
        {
            var usuarioOrganizadorExistente = _context.GrupoOrganizadors.FirstOrDefault(i => i.IdEvento == idEvento && i.IdUsuario == idUsuario);
            if (usuarioOrganizadorExistente != null)
            {
                UsuarioG.mensajeError = "El usuario ya forma parte de la lista.";
                return RedirectToAction("IndexGO", "InscripcionA", new { id = idEvento });
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario == null) {
                UsuarioG.mensajeError = "El usuario no existe.";
                return RedirectToAction("IndexGO", "InscripcionA", new { id = idEvento });
            }

            GrupoOrganizador grupoOrganizador = new GrupoOrganizador
            {
                IdEvento = idEvento,
                IdUsuario = idUsuario
            };

            if (ModelState.IsValid)
            {
                UsuarioG.mensajeError = "Se agrego correctamente el usuario.";
                _context.Add(grupoOrganizador);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexGO", "InscripcionA", new { id = idEvento });
            }

            UsuarioG.mensajeError = "Ocurrio un error. El usuario no existe o se ha presentado otro problema.";
            return RedirectToAction("IndexGO", "InscripcionA", new { id = idEvento });
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

        [HttpPost]
        public async Task<IActionResult> DeleteGO(int idEvento, int idUsuario)
        {
            var grupoOrganizador = await _context.GrupoOrganizadors.FirstOrDefaultAsync(i => i.IdEvento == idEvento && i.IdUsuario == idUsuario);

            // Verificar si la entidad existe
            if (grupoOrganizador == null)
            {
                TempData["ErrorMessage"] = "No se encontró el grupo organizador para eliminar.";
                return RedirectToAction(nameof(Index));
            }

            // Eliminar la entidad encontrada
            _context.GrupoOrganizadors.Remove(grupoOrganizador);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Redirigir a la página principal o a otra página según sea necesario
            return RedirectToAction("IndexGO", "InscripcionA", new { id = idEvento });
        }


        private bool InscripcionExists(int id)
        {
            return _context.Inscripcions.Any(e => e.IdInscripcion == id);
        }
    }
}
