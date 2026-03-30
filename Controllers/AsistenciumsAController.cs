using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OEED_ITT.Helpers;
using OEED_ITT.Models;

namespace OEED_ITT.Controllers
{
    public class AsistenciumsAController : Controller
    {
        private readonly EventosInstitucionalesContext _context;

        public AsistenciumsAController(EventosInstitucionalesContext context)
        {
            _context = context;
        }

        // GET: Asistenciums
        public async Task<IActionResult> Index(int idEvento)
        {
            var eventosInstitucionalesContext = _context.Asistencia.Include(a => a.IdEventoNavigation).Include(a => a.IdUsuarioNavigation).Include(a => a.IdUsuarioNavigation.IddepartamentoNavigation).Where(a => a.IdEvento == idEvento);
            ViewData["idEvento"] = idEvento;
            return View(await eventosInstitucionalesContext.ToListAsync());
        }

        public async Task<IActionResult> IndexAC(int idEvento)
        {
            var eventosInstitucionalesContext = _context.Asistencia.Include(a => a.IdEventoNavigation).Include(a => a.IdUsuarioNavigation).Include(a => a.IdUsuarioNavigation.IddepartamentoNavigation).Where(a => a.IdEvento == idEvento);
            ViewData["idEvento"] = idEvento;
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

        public IActionResult GenerarExcel(int idEvento)
        {
            var asistencias = // Obtén la lista de asistencias del evento con idEvento desde tu base de datos
                _context.Asistencia
                .Where(a => a.IdEvento == idEvento)
                .Select(a => new
                {
                    a.IdUsuarioNavigation.IdUsuario,
                    a.IdUsuarioNavigation.NombreUsuario,
                    a.IdUsuarioNavigation.ApellidoUsuario,
                    a.IdUsuarioNavigation.IddepartamentoNavigation.Nombre,
                    a.HoraAsistencia
                })
                .ToList();

            var evento = _context.Eventos.Where(e => e.IdEvento == idEvento).FirstOrDefault();
            string nombreArchivo = evento.NombreEvento + "_ListaAsistencia.xlsx";

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Asistencias");
                worksheet.Cell(1, 1).Value = "No. de control";
                worksheet.Cell(1, 2).Value = "Nombre";
                worksheet.Cell(1, 3).Value = "Apellido";
                worksheet.Cell(1, 4).Value = "Departamento";
                worksheet.Cell(1, 5).Value = "Hora de asistencia";

                for (int i = 0; i < asistencias.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = asistencias[i].IdUsuario;
                    worksheet.Cell(i + 2, 2).Value = asistencias[i].NombreUsuario;
                    worksheet.Cell(i + 2, 3).Value = asistencias[i].ApellidoUsuario;
                    worksheet.Cell(i + 2, 4).Value = asistencias[i].Nombre;
                    worksheet.Cell(i + 2, 5).Value = asistencias[i].HoraAsistencia;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
        }

        public async Task<IActionResult> CreatePost(int idEvento, int idUsuario)
        {
            var asistenciaExistente = _context.Asistencia.FirstOrDefault(i => i.IdEvento == idEvento && i.IdUsuario == idUsuario);
            if (asistenciaExistente != null)
            {
                UsuarioG.mensajeError = "El usuario ya esta registrado en la lista de asistencia";
                return RedirectToAction("Index", "AsistenciumsO", new { idEvento = idEvento });
            }

            var usuarioInexistente = _context.Usuarios.FirstOrDefault(i => i.IdUsuario == idUsuario);
            if (usuarioInexistente == null)
            {
                UsuarioG.mensajeError = "El usuario no existe o no esta registrado en el sistema";
                return RedirectToAction("Index", "AsistenciumsO", new { idEvento = idEvento });
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
                return RedirectToAction("Index", "AsistenciumsO", new {idEvento = idEvento});
            }

            UsuarioG.mensajeError = "Se presento un error inesperado";
            return RedirectToAction("Index", "AsistenciumsO", new { idEvento = idEvento });
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
                return RedirectToAction("Index", "Asistenciums", idEvento);
            }

            // Eliminar la entidad encontrada
            _context.Asistencia.Remove(asistencia);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Redirigir a la página principal o a otra página según sea necesario
            return RedirectToAction("Index", "Asistenciums",idEvento);
        }

        private bool AsistenciumExists(int id)
        {
            return _context.Asistencia.Any(e => e.IdAsistencia == id);
        }
    }
}
