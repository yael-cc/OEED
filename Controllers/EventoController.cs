using System;
using System.IO;
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
    public class EventoController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public EventoController(EventosInstitucionalesContext context)
        {
            _context = context;
        }


        // GET: Evento
        public async Task<IActionResult> Index()
        {
            UsuarioG.ActualizarEstadoEventos(_context);
            DateTime ahora = DateTime.Now;
            List<Evento> lista = await _context.Eventos
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento)
                .Where(e => e.IdEstadoEvento == 1 && ahora < e.HoraInicioEvento) // Agrega el filtro aquí
                .ToListAsync();

            return View(lista);
        }

        public IActionResult HomeIndex()
        {
            UsuarioG.ActualizarEstadoEventos(_context);
            ViewData["IdUsuario"] = UsuarioG.IdUsuario;
            ViewData["NombreCompleto"] = (UsuarioG.ApellidoUsuario + " " + UsuarioG.NombreUsuario + "");
            ViewData["NombreDepartamento"] = UsuarioG.NombreDepartamento;

            var evento = (from e in _context.Eventos.Include(e => e.oDepartamento).Include(e => e.oEstado).Include(e => e.oLugar).Include(e => e.oTipoEvento)
                          join i in _context.Inscripcions
                          on e.IdEvento equals i.IdEvento
                          where i.IdUsuario == UsuarioG.IdUsuario && i.IdEstadoInscripcion == 1 && e.IdEstadoEvento == 1
                          orderby e.FechaEvento
                          select e).FirstOrDefault();

            return View(evento);
        }

        public async Task<IActionResult> IndexD()
        {
            DateTime ahora = DateTime.Now;
            var eventos = from e in _context.Eventos
                          where (from di in _context.DepartamentoInvitados
                                 where di.IdDepartamento == UsuarioG.Iddepartamento
                                 select di.IdEvento)
                                 .Contains(e.IdEvento) || !_context.DepartamentoInvitados.Any(di => di.IdEvento == e.IdEvento)
                          select e;

            eventos = eventos.Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento)
                .Where(e => e.IdEstadoEvento == 1 && ahora < e.HoraInicioEvento);

            List<Evento> listaEventos = eventos.ToList();
            return View(listaEventos);
        }

        public async Task<IActionResult> IndexStaff(int idUsuario)
        {
            DateTime ahora = DateTime.Now;
            var eventos = from e in _context.Eventos
                          where (from go in _context.GrupoOrganizadors
                                 where go.IdUsuario == UsuarioG.IdUsuario
                                 select go.IdEvento)
                                 .Contains(e.IdEvento)
                          select e;

            eventos = eventos.Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento);

            eventos = eventos.Where(e => e.HoraInicioEvento  <= ahora  && e.HoraFinEvento >= ahora);
            ViewData["IdUsuario"] = idUsuario;
            List <Evento> listaEventos = eventos.ToList();

            return View(eventos);
        }

        public async Task<IActionResult> IndexFin(int idUsuario)
        {
            DateTime ahora = DateTime.Now;
            var eventos = from e in _context.Eventos
                          where (from go in _context.GrupoOrganizadors
                                 where go.IdUsuario == UsuarioG.IdUsuario
                                 select go.IdEvento)
                                 .Contains(e.IdEvento)
                          select e;

            eventos = eventos.Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento);

            eventos = eventos.Where(e => e.IdEstadoEvento == 4);

            List<Evento> listaEventos = eventos.ToList();

            return View(eventos);
        }

        public async Task<IActionResult> IndexEF()
        {
            var eventosConAsistencia = from evento in _context.Eventos
                                       where (from asistencia in _context.Asistencia
                                              where asistencia.IdUsuario == UsuarioG.IdUsuario
                                              select asistencia.IdEvento)
                                             .Contains(evento.IdEvento)
                                       select evento;

            List<Evento> listaEventos = eventosConAsistencia.ToList();

            return View(eventosConAsistencia);
        }

        // GET: Evento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento)
                .Include(e => e.Inscripcions)
                .FirstOrDefaultAsync(m => m.IdEvento == id);
        
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // GET: Evento/Create
        public IActionResult Create()
        {
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "IdDepartamento");
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "IdEstadoEvento");
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "IdLugar");
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "IdTipoEvento");
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEvento,NombreEvento,DescripcionEvento,FechaEvento,IdTipoEvento,HoraInicioEvento,HoraFinEvento,IdlugarEvento,IddepartamentoOrigen,IdEstadoEvento,NumMaxEvento,Imagen")] Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "IdDepartamento", evento.IddepartamentoOrigen);
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "IdEstadoEvento", evento.IdEstadoEvento);
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "IdLugar", evento.IdlugarEvento);
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "IdTipoEvento", evento.IdTipoEvento);
            return View(evento);
        }

        // GET: Evento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "IdDepartamento", evento.IddepartamentoOrigen);
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "IdEstadoEvento", evento.IdEstadoEvento);
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "IdLugar", evento.IdlugarEvento);
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "IdTipoEvento", evento.IdTipoEvento);
            return View(evento);
        }

        // POST: Evento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile imagenFile, [Bind("IdEvento,NombreEvento,DescripcionEvento,FechaEvento,IdTipoEvento,HoraInicioEvento,HoraFinEvento,IdlugarEvento,IddepartamentoOrigen,IdEstadoEvento,NumMaxEvento,Imagen")] Evento evento)
        {
            var fileContentBase64 = "";
            if (id != evento.IdEvento)
            {
                return NotFound();
            }

            if (imagenFile != null && imagenFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagenFile.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    fileContentBase64 = Convert.ToBase64String(fileBytes);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    evento.Imagen = fileContentBase64;
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.IdEvento))
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
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", evento.IddepartamentoOrigen);
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "NombreEstadoEvento", evento.IdEstadoEvento);
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "NombreLugar", evento.IdlugarEvento);
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "NombreTipoEvento", evento.IdTipoEvento);
            return View(evento);
        }

        // GET: Evento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evento = await _context.Eventos
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento)
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.IdEvento == id);
        }
    }
}
