using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaChequesNuevo.Data;
using SistemaChequesNuevo.Dtos;
using SistemaChequesNuevo.Helpers;
using SistemaChequesNuevo.Models;
namespace SistemaChequesNuevo.Controllers
{
    [Authorize]
    public class SolicitudChequeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SolicitudChequeController(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<SelectList> GetCuentaContableAsync()
        {
            using var client = new HttpClient();
            var cuentasContablesResponse = await client.GetAsync(ContabilidadAPIConstans.CUENTAS_CONTABLES);
            var cuentasContablesResponseBody = cuentasContablesResponse.IsSuccessStatusCode ? await cuentasContablesResponse.Content.ReadAsStringAsync() : "";
            var cuentasContables = JsonConvert.DeserializeObject<CuentaContableResponse>(cuentasContablesResponseBody);
            return new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Cuenta Contable" } }.Concat(cuentasContables.cuentasContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");
        }
        // GET: SolicitudCheque
        public async Task<IActionResult> Index()
        {
            var cuentasContables = await GetCuentaContableAsync();
            var applicationDbContext = _context.SolicitudCheques.Include(s => s.Proveedor);
            var solicitudes = await applicationDbContext.ToListAsync();
            var cuentasContablesList = cuentasContables.Where(x => !String.IsNullOrEmpty(x.Value)).ToList();

            foreach (var solicitud in solicitudes)
            {
                solicitud.CuentaContableDescription = cuentasContablesList.FirstOrDefault(x => Convert.ToInt32(x.Value) == solicitud?.CuentaContable)?.Text ?? "";
            }

            var providers = _context.Proveedores.ToList();
            var defaultOption = new SelectListItem() { Value = "", Text = "Select a provider" };
            var selectList = new SelectList(new List<SelectListItem> { defaultOption }.Concat(providers.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })), "Value", "Text");
            ViewBag.ProveedorId = selectList;
            ViewBag.cuentasContables = cuentasContables;
            var dto = new SolicitudCheque();
            dto.Solicitudes = solicitudes;
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Search(SolicitudCheque body)
        {
            var cuentasContables = await GetCuentaContableAsync();
            IQueryable<SolicitudCheque> applicationDbContext = _context.SolicitudCheques.Include(s => s.Proveedor);
            if (body.ProveedorId != null)
            {
                applicationDbContext = applicationDbContext.Where(s => s.ProveedorId == body.ProveedorId);
            }
            if (body.FechaDesde != null && body.FechaHasta != null && body.FechaDesde < body.FechaHasta)
            {
                applicationDbContext = applicationDbContext.Where(s => s.FechaRegistro >= body.FechaDesde && s.FechaRegistro <= body.FechaHasta);
            }
            if (body.CuentaContable != null)
            {
                applicationDbContext = applicationDbContext.Where(s => s.CuentaContable == body.CuentaContable);
            }
            var solicitudes = await applicationDbContext.ToListAsync();
            var cuentasContablesList = cuentasContables.Where(x => !String.IsNullOrEmpty(x.Value)).ToList();
            foreach (var solicitud in solicitudes)
            {
                solicitud.CuentaContableDescription = cuentasContablesList.FirstOrDefault(x => Convert.ToInt32(x.Value) == solicitud.CuentaContable).Text;
            }
            var providers = _context.Proveedores.ToList();
            var defaultOption = new SelectListItem() { Value = "", Text = "Select a provider" };
            var selectList = new SelectList(new List<SelectListItem> { defaultOption }.Concat(providers.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })), "Value", "Text");
            ViewBag.ProveedorId = selectList;
            ViewBag.cuentasContables = cuentasContables;
            var dto = new SolicitudCheque();
            dto.Solicitudes = solicitudes;
            return View("Index", dto);
        }
        // GET: SolicitudCheque/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SolicitudCheques == null)
            {
                return NotFound();
            }
            var solicitudCheque = await _context.SolicitudCheques
                .Include(s => s.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitudCheque == null)
            {
                return NotFound();
            }
            return View(solicitudCheque);
        }
        // GET: SolicitudCheque/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.cuentasContables = await GetCuentaContableAsync();
            var providers = _context.Proveedores.ToList();
            var defaultOption = new SelectListItem() { Value = "", Text = "Select a provider" };
            var selectList = new SelectList(new List<SelectListItem> { defaultOption }.Concat(providers.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Nombre })), "Value", "Text");
            ViewBag.ProveedorId = selectList;
            return View();
        }
        // POST: SolicitudCheque/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroSolicitud,Monto,FechaRegistro,Estado,CuentaContable,CuentaDestino,ProveedorId")] SolicitudCheque solicitudCheque)
        {
            if (ModelState.IsValid)
            {
                var totalCheckes = _context.SolicitudCheques.OrderByDescending(x => x.NumeroSolicitud)?.FirstOrDefault()?.NumeroSolicitud ?? 0;
                solicitudCheque.NumeroSolicitud = totalCheckes + 1;
                _context.Add(solicitudCheque);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Id", solicitudCheque.ProveedorId);
            return View(solicitudCheque);
        }
        // GET: SolicitudCheque/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SolicitudCheques == null)
            {
                return NotFound();
            }
            var solicitudCheque = await _context.SolicitudCheques.FindAsync(id);
            if (solicitudCheque == null)
            {
                return NotFound();
            }
            ViewBag.cuentasContables = await GetCuentaContableAsync();
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Id", solicitudCheque.ProveedorId);
            return View(solicitudCheque);
        }
        // POST: SolicitudCheque/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroSolicitud,Monto,FechaRegistro,Estado,CuentaContable,CuentaDestino,ProveedorId")] SolicitudCheque solicitudCheque)
        {
            if (id != solicitudCheque.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    solicitudCheque.NumeroSolicitud = _context.SolicitudCheques.AsNoTracking().FirstOrDefault(x => x.Id == id).NumeroSolicitud;
                    _context.Update(solicitudCheque);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudChequeExists(solicitudCheque.Id))
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
            ViewData["ProveedorId"] = new SelectList(_context.Proveedores, "Id", "Id", solicitudCheque.ProveedorId);
            return View(solicitudCheque);
        }
        // GET: SolicitudCheque/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SolicitudCheques == null)
            {
                return NotFound();
            }
            var solicitudCheque = await _context.SolicitudCheques
                .Include(s => s.Proveedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitudCheque == null)
            {
                return NotFound();
            }
            return View(solicitudCheque);
        }
        // POST: SolicitudCheque/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SolicitudCheques == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SolicitudCheques'  is null.");
            }
            var solicitudCheque = await _context.SolicitudCheques.FindAsync(id);
            if (solicitudCheque != null)
            {
                _context.SolicitudCheques.Remove(solicitudCheque);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SolicitudChequeExists(int id)
        {
            return (_context.SolicitudCheques?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}