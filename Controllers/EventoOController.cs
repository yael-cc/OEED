using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Helpers;
using OEED_ITT.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace OEED_ITT.Controllers
{
    public class EventoOController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public EventoOController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            UsuarioG.ActualizarEstadoEventos(_context);
            var eventosInstitucionalesContext = _context.Eventos
                .Where(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && e.IdEstadoEvento == 1)
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento);

            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        public IActionResult HomeIndex()
        {
            UsuarioG.ActualizarEstadoEventos(_context);
            ViewData["IdUsuario"] = UsuarioG.IdUsuario;
            ViewData["NombreCompleto"] = (UsuarioG.ApellidoUsuario + " " + UsuarioG.NombreUsuario);
            ViewData["NombreDepartamento"] = UsuarioG.NombreDepartamento;

            var evento = _context.Eventos
                        .Include(e => e.oDepartamento)
                        .Include(e => e.oEstado)
                        .Include(e => e.oLugar)
                        .Include(e => e.oTipoEvento)
                        .Where(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && (e.IdEstadoEvento < 3))
                        .OrderBy(e => e.FechaEvento)
                        .FirstOrDefault();


            return View(evento);
        }


        public IActionResult Estadisticas()
        {
            var eventosRealizados = _context.Eventos
                .Count(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento);
            var eventosFinalizados = _context.Eventos
                .Count(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && e.IdEstadoEvento == 4);
            var eventosCancelados = _context.Eventos
                .Count(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && e.IdEstadoEvento == 3);
            var eventosProgresoOFuturos = _context.Eventos
                .Count(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && (e.IdEstadoEvento == 1 || e.IdEstadoEvento == 2));
            var totalInscripciones = _context.Inscripcions
                .Count(i => _context.Usuarios.Any(u => u.IdUsuario == i.IdUsuario && u.Iddepartamento == UsuarioG.Iddepartamento));
            var totalAsistenciasEventosDepartamento = _context.Asistencia
                .Count(a => _context.Eventos.Any(e => e.IdEvento == a.IdEvento && e.IddepartamentoOrigen == UsuarioG.Iddepartamento));
            var asistenciasDepartamentoEnSusEventos = _context.Asistencia
                .Count(a => _context.Usuarios.Any(u => u.IdUsuario == a.IdUsuario && u.Iddepartamento == UsuarioG.Iddepartamento) &&
                _context.Eventos.Any(e => e.IdEvento == a.IdEvento && e.IddepartamentoOrigen == UsuarioG.Iddepartamento));
            var asistenciasOtroDepartamento = _context.Asistencia
                .Count(a => !_context.Usuarios.Any(u => u.IdUsuario == a.IdUsuario && u.Iddepartamento == UsuarioG.Iddepartamento) &&
                _context.Eventos.Any(e => e.IdEvento == a.IdEvento && e.IddepartamentoOrigen == UsuarioG.Iddepartamento));

            ViewData["eventosRealizados"] = eventosRealizados;
            ViewData["eventosFinalizados"] = eventosFinalizados;
            ViewData["eventosCancelados"] = eventosCancelados;
            ViewData["eventosProgresoOFuturos"] = eventosProgresoOFuturos;
            ViewData["totalInscripciones"] = totalInscripciones;
            ViewData["totalAsistenciasEventosDepartamento"] = totalAsistenciasEventosDepartamento;
            ViewData["asistenciasDepartamentoEnSusEventos"] = asistenciasDepartamentoEnSusEventos;
            ViewData["asistenciasOtroDepartamento"] = asistenciasOtroDepartamento;

            return View();
        }

        public async Task<IActionResult> IndexDI(int idEvento)
        {
            var eventosInstitucionalesContext = _context.DepartamentoInvitados
                .Where(e => e.IdEvento == idEvento)
                .Include(e => e.IdEventoNavigation)
                .Include(e => e.IdDepartamentoNavigation);
            ViewData["idEvento"] = idEvento;
            ViewData["Iddepartamento"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");

            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        public async Task<IActionResult> IndexE(int idUsuario)
        {
            UsuarioG.ActualizarEstadoEventos(_context);
            var eventosInstitucionalesContext = _context.Eventos
                .Where(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && e.IdEstadoEvento == 2)
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento);

            ViewData["IdUsuario"] = idUsuario;

            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        public async Task<IActionResult> IndexFin()
        {
            UsuarioG.ActualizarEstadoEventos(_context);
            var eventosInstitucionalesContext = _context.Eventos
                .Where(e => e.IddepartamentoOrigen == UsuarioG.Iddepartamento && e.IdEstadoEvento == 4)
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento);

            return View(await eventosInstitucionalesContext.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.IdEvento == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        public async Task<IActionResult> EventoConfirmado(int idEvento)
        {
            if (idEvento == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var evento =  await _context.Eventos
                .Include(e => e.oDepartamento)
                .Include(e => e.oEstado)
                .Include(e => e.oLugar)
                .Include(e => e.oTipoEvento)
                .FirstOrDefaultAsync(m => m.IdEvento == idEvento);

            ViewData["idEvento"]= idEvento;

            if (evento == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }



        // GET: Evento/Create
        public IActionResult Create()
        {
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre");
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "NombreEstadoEvento");
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "NombreLugar");
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "NombreTipoEvento");
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile imagenFile, [Bind("IdEvento,NombreEvento,DescripcionEvento,FechaEvento,IdTipoEvento,HoraInicioEvento,HoraFinEvento,IdlugarEvento,IddepartamentoOrigen,IdEstadoEvento,NumMaxEvento,Imagen")] Evento evento)
        {
            var fileContentBase64 = "";

            if (imagenFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagenFile.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    fileContentBase64 = Convert.ToBase64String(fileBytes);
                    evento.Imagen = fileContentBase64;
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(evento);
                await _context.SaveChangesAsync();
                AgregarGO(evento.IdEvento);

                return RedirectToAction("EventoConfirmado", "EventoO", new { idEvento = evento.IdEvento});
            }
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", evento.IddepartamentoOrigen);
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "NombreEstadoEvento", evento.IdEstadoEvento);
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "NombreLugar", evento.IdlugarEvento);
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "NombreTipoEvento", evento.IdTipoEvento);
            return View(evento);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDI(int idEvento, string idDepartamento)
        {
            var departamentoInvitadoExistente = _context.DepartamentoInvitados.FirstOrDefault(i => i.IdEvento == idEvento && i.IdDepartamento == idDepartamento);
            if (departamentoInvitadoExistente != null)
            {
                UsuarioG.mensajeError = "El departamento ya forma parte de los invitados.";
                return RedirectToAction("IndexDI", "EventoO", new { idEvento = idEvento });
            }

            DepartamentoInvitado departamentoInvitado = new DepartamentoInvitado
            {
                IdEvento = idEvento,
                IdDepartamento = idDepartamento
            };

            if (ModelState.IsValid)
            {
                UsuarioG.mensajeError = "El departamento se ha agregado a los invitados.";
                _context.Add(departamentoInvitado);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexDI", "EventoO", new { idEvento = idEvento });
            }

            UsuarioG.mensajeError = "El departamento no existe u ocurrio algun error inesperado.";
            return RedirectToAction("IndexDI", "EventoO", new { idEvento = idEvento });
        }

        public void AgregarGO(int idEvento) {
            GrupoOrganizador go = new GrupoOrganizador
            {
                IdUsuario = UsuarioG.IdUsuario,
                IdEvento = idEvento
            };
            _context.Add(go);
            _context.SaveChangesAsync();
        }

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
            ViewData["IddepartamentoOrigen"] = new SelectList(_context.Departamentos, "IdDepartamento", "Nombre", evento.IddepartamentoOrigen);
            ViewData["IdEstadoEvento"] = new SelectList(_context.EstadoEventos, "IdEstadoEvento", "NombreEstadoEvento", evento.IdEstadoEvento);
            ViewData["IdlugarEvento"] = new SelectList(_context.Lugars, "IdLugar", "NombreLugar", evento.IdlugarEvento);
            ViewData["IdTipoEvento"] = new SelectList(_context.TipoEventos, "IdTipoEvento", "NombreTipoEvento", evento.IdTipoEvento);
            return View(evento);
        }

        // POST: Evento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile? imagenFile, [Bind("IdEvento,NombreEvento,DescripcionEvento,FechaEvento,IdTipoEvento,HoraInicioEvento,HoraFinEvento,IdlugarEvento,IddepartamentoOrigen,IdEstadoEvento,NumMaxEvento,Imagen")] Evento evento)
        {
            var fileContentBase64 = "";
            if (id != evento.IdEvento)
            {
                return NotFound();
            }

            if (imagenFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagenFile.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    fileContentBase64 = Convert.ToBase64String(fileBytes);
                    evento.Imagen = fileContentBase64;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        public ActionResult Cancelar(int idEvento)
        {
            var evento = _context.Eventos.FirstOrDefault(e => e.IdEvento == idEvento);
            if (evento == null)
            {
                return NotFound(); // O manejar el error de alguna otra manera
            }

            ViewData["IdEvento"] = idEvento;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CancelarEvento(int idEvento)
        {
            var evento = await _context.Eventos
                .FirstOrDefaultAsync(eu => eu.IdEvento == idEvento);

            if (evento == null)
            {
                return RedirectToAction("Index", "EventoO");
            }

            evento.IdEstadoEvento = 3;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction("Index", "EventoO");
        }

        private bool EventoExists(int id)
        {
            return _context.Eventos.Any(e => e.IdEvento == id);
        }
    }
}
