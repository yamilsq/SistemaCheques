using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaChequesNuevo.Data;
using SistemaChequesNuevo.Helpers;
using SistemaChequesNuevo.Models;
using Newtonsoft.Json;
using SistemaChequesNuevo.Dtos;
using System.Security.Cryptography.Xml;

namespace SistemaChequesNuevo.Controllers
{
    [Authorize]
    public class AsientoContableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AsientoContableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pago
        public async Task<IActionResult> Index()
        {
            using var client = new HttpClient();

            // Start all the asynchronous operations concurrently
            var estadosContablesTask = client.GetAsync(ContabilidadAPIConstans.ESTADOS_CONTABLES);
            var monedaTask = client.GetAsync(ContabilidadAPIConstans.MONEDAS);
            var cuentasContablesTask = client.GetAsync(ContabilidadAPIConstans.CUENTAS_CONTABLES);
            var tiposDeCuentaTask = client.GetAsync(ContabilidadAPIConstans.TIPOS_DE_CUENTA);
            var origenCuentaTask = client.GetAsync(ContabilidadAPIConstans.ORIGEN_CUENTA);

            // Await for all tasks to complete
            await Task.WhenAll(estadosContablesTask, monedaTask, cuentasContablesTask, tiposDeCuentaTask, origenCuentaTask);

            // Retrieve the responses
            var estadosContablesResponse = await estadosContablesTask;
            var monedaResponse = await monedaTask;
            var cuentasContablesResponse = await cuentasContablesTask;
            var tiposDeCuentaResponse = await tiposDeCuentaTask;
            var origenCuentaResponse = await origenCuentaTask;

            // Read response bodies if successful
            var estadosContablesResponseBody = estadosContablesResponse.IsSuccessStatusCode ? await estadosContablesResponse.Content.ReadAsStringAsync() : "";
            var monedaResponseBody = monedaResponse.IsSuccessStatusCode ? await monedaResponse.Content.ReadAsStringAsync() : "";
            var cuentasContablesResponseBody = cuentasContablesResponse.IsSuccessStatusCode ? await cuentasContablesResponse.Content.ReadAsStringAsync() : "";
            var tiposDeCuentaResponseBody = tiposDeCuentaResponse.IsSuccessStatusCode ? await tiposDeCuentaResponse.Content.ReadAsStringAsync() : "";
            var origenCuentaResponseBody = origenCuentaResponse.IsSuccessStatusCode ? await origenCuentaResponse.Content.ReadAsStringAsync() : "";

            var estadosContable = JsonConvert.DeserializeObject<EstadoContable>(estadosContablesResponseBody);
            var monedas = JsonConvert.DeserializeObject<MonedasResponse>(monedaResponseBody);
            var cuentasContables = JsonConvert.DeserializeObject<CuentaContableResponse>(cuentasContablesResponseBody);
            var tiposDeCuentaContables = JsonConvert.DeserializeObject<TipoCuentaReponse>(tiposDeCuentaResponseBody);
            var origenCuenta = JsonConvert.DeserializeObject<OrigenCuentaReponse>(origenCuentaResponseBody);

            ViewBag.estadosContables = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione un Estado Contable" } }.Concat(estadosContable?.estadosContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");
            ViewBag.monedas = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Moneda" } }.Concat(monedas?.Monedas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Descripcion })), "Value", "Text");
            ViewBag.cuentasContables = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Cuenta Contable" } }.Concat(cuentasContables.cuentasContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");

            var origenCuentaUIMapper = cuentasContables.cuentasContables.Select(x =>
                new OrigenCuentaUIMapper {
                    cuentaContableId = x.id,
                    origenCuentaDescripcion = origenCuenta?.cuentasContables.FirstOrDefault(o => o.id == tiposDeCuentaContables?.cuentasContables.FirstOrDefault(y => y.id == x.tipo).origen).descripcion
                }
            ).ToList();

            ViewBag.origenCuentaUIMapper = origenCuentaUIMapper;

            var model = new AsientoContable();
            model.Transacciones = new List<Transaccion> {
                    new Transaccion {},
                    new Transaccion {}
            };
            return View(model);
        }

        [HttpPost("AsientoContable/OnRemoveTransaction/{id}")]
        public async Task<IActionResult> OnRemoveTransaction(AsientoContable model, int id)
        {
            using var client = new HttpClient();

            // Start all the asynchronous operations concurrently
            var estadosContablesTask = client.GetAsync(ContabilidadAPIConstans.ESTADOS_CONTABLES);
            var monedaTask = client.GetAsync(ContabilidadAPIConstans.MONEDAS);
            var cuentasContablesTask = client.GetAsync(ContabilidadAPIConstans.CUENTAS_CONTABLES);
            var tiposDeCuentaTask = client.GetAsync(ContabilidadAPIConstans.TIPOS_DE_CUENTA);
            var origenCuentaTask = client.GetAsync(ContabilidadAPIConstans.ORIGEN_CUENTA);

            // Await for all tasks to complete
            await Task.WhenAll(estadosContablesTask, monedaTask, cuentasContablesTask, tiposDeCuentaTask, origenCuentaTask);

            // Retrieve the responses
            var estadosContablesResponse = await estadosContablesTask;
            var monedaResponse = await monedaTask;
            var cuentasContablesResponse = await cuentasContablesTask;
            var tiposDeCuentaResponse = await tiposDeCuentaTask;
            var origenCuentaResponse = await origenCuentaTask;

            // Read response bodies if successful
            var estadosContablesResponseBody = estadosContablesResponse.IsSuccessStatusCode ? await estadosContablesResponse.Content.ReadAsStringAsync() : "";
            var monedaResponseBody = monedaResponse.IsSuccessStatusCode ? await monedaResponse.Content.ReadAsStringAsync() : "";
            var cuentasContablesResponseBody = cuentasContablesResponse.IsSuccessStatusCode ? await cuentasContablesResponse.Content.ReadAsStringAsync() : "";
            var tiposDeCuentaResponseBody = tiposDeCuentaResponse.IsSuccessStatusCode ? await tiposDeCuentaResponse.Content.ReadAsStringAsync() : "";
            var origenCuentaResponseBody = origenCuentaResponse.IsSuccessStatusCode ? await origenCuentaResponse.Content.ReadAsStringAsync() : "";

            var estadosContable = JsonConvert.DeserializeObject<EstadoContable>(estadosContablesResponseBody);
            var monedas = JsonConvert.DeserializeObject<MonedasResponse>(monedaResponseBody);
            var cuentasContables = JsonConvert.DeserializeObject<CuentaContableResponse>(cuentasContablesResponseBody);
            var tiposDeCuentaContables = JsonConvert.DeserializeObject<TipoCuentaReponse>(tiposDeCuentaResponseBody);
            var origenCuenta = JsonConvert.DeserializeObject<OrigenCuentaReponse>(origenCuentaResponseBody);

            ViewBag.estadosContables = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione un Estado Contable" } }.Concat(estadosContable?.estadosContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");
            ViewBag.monedas = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Moneda" } }.Concat(monedas?.Monedas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Descripcion })), "Value", "Text");
            ViewBag.cuentasContables = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Cuenta Contable" } }.Concat(cuentasContables.cuentasContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");

            var origenCuentaUIMapper = cuentasContables.cuentasContables.Select(x =>
                new OrigenCuentaUIMapper
                {
                    cuentaContableId = x.id,
                    origenCuentaDescripcion = origenCuenta?.cuentasContables.FirstOrDefault(o => o.id == tiposDeCuentaContables?.cuentasContables.FirstOrDefault(y => y.id == x.tipo).origen).descripcion
                }
            ).ToList();

            model.Fecha = new DateTime();
            ViewBag.origenCuentaUIMapper = origenCuentaUIMapper;
            model.Transacciones.RemoveAt(id);
            return View("Index", model);
        }

        [HttpPost("AsientoContable/OnAddTransaction")]
        public async Task<IActionResult> OnAddTransaction(AsientoContable model) 
        {
            using var client = new HttpClient();

            // Start all the asynchronous operations concurrently
            var estadosContablesTask = client.GetAsync(ContabilidadAPIConstans.ESTADOS_CONTABLES);
            var monedaTask = client.GetAsync(ContabilidadAPIConstans.MONEDAS);
            var cuentasContablesTask = client.GetAsync(ContabilidadAPIConstans.CUENTAS_CONTABLES);
            var tiposDeCuentaTask = client.GetAsync(ContabilidadAPIConstans.TIPOS_DE_CUENTA);
            var origenCuentaTask = client.GetAsync(ContabilidadAPIConstans.ORIGEN_CUENTA);

            // Await for all tasks to complete
            await Task.WhenAll(estadosContablesTask, monedaTask, cuentasContablesTask, tiposDeCuentaTask, origenCuentaTask);

            // Retrieve the responses
            var estadosContablesResponse = await estadosContablesTask;
            var monedaResponse = await monedaTask;
            var cuentasContablesResponse = await cuentasContablesTask;
            var tiposDeCuentaResponse = await tiposDeCuentaTask;
            var origenCuentaResponse = await origenCuentaTask;

            // Read response bodies if successful
            var estadosContablesResponseBody = estadosContablesResponse.IsSuccessStatusCode ? await estadosContablesResponse.Content.ReadAsStringAsync() : "";
            var monedaResponseBody = monedaResponse.IsSuccessStatusCode ? await monedaResponse.Content.ReadAsStringAsync() : "";
            var cuentasContablesResponseBody = cuentasContablesResponse.IsSuccessStatusCode ? await cuentasContablesResponse.Content.ReadAsStringAsync() : "";
            var tiposDeCuentaResponseBody = tiposDeCuentaResponse.IsSuccessStatusCode ? await tiposDeCuentaResponse.Content.ReadAsStringAsync() : "";
            var origenCuentaResponseBody = origenCuentaResponse.IsSuccessStatusCode ? await origenCuentaResponse.Content.ReadAsStringAsync() : "";

            var estadosContable = JsonConvert.DeserializeObject<EstadoContable>(estadosContablesResponseBody);
            var monedas = JsonConvert.DeserializeObject<MonedasResponse>(monedaResponseBody);
            var cuentasContables = JsonConvert.DeserializeObject<CuentaContableResponse>(cuentasContablesResponseBody);
            var tiposDeCuentaContables = JsonConvert.DeserializeObject<TipoCuentaReponse>(tiposDeCuentaResponseBody);
            var origenCuenta = JsonConvert.DeserializeObject<OrigenCuentaReponse>(origenCuentaResponseBody);

            ViewBag.estadosContables = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione un Estado Contable" } }.Concat(estadosContable?.estadosContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");
            ViewBag.monedas = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Moneda" } }.Concat(monedas?.Monedas.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Descripcion })), "Value", "Text");
            ViewBag.cuentasContables = new SelectList(new List<SelectListItem> { new SelectListItem() { Value = "", Text = "Seleccione una Cuenta Contable" } }.Concat(cuentasContables.cuentasContables.Select(p => new SelectListItem { Value = p.id.ToString(), Text = p.descripcion })), "Value", "Text");

            var origenCuentaUIMapper = cuentasContables.cuentasContables.Select(x =>
                new OrigenCuentaUIMapper
                {
                    cuentaContableId = x.id,
                    origenCuentaDescripcion = origenCuenta?.cuentasContables.FirstOrDefault(o => o.id == tiposDeCuentaContables?.cuentasContables.FirstOrDefault(y => y.id == x.tipo).origen).descripcion
                }
            ).ToList();

            ViewBag.origenCuentaUIMapper = origenCuentaUIMapper;
            model.Transacciones.Add(new Transaccion { });
            return View("Index", model);
        }

        // GET: Pago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pagos == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("AsientoContable/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descripcion,Estado, Moneda, Fecha, Monto, Transacciones")] AsientoContable pago)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(pago);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}

            using var client = new HttpClient();

            var cuentasContablesTask = client.GetAsync(ContabilidadAPIConstans.CUENTAS_CONTABLES);
            var tiposDeCuentaTask = client.GetAsync(ContabilidadAPIConstans.TIPOS_DE_CUENTA);
            var origenCuentaTask = client.GetAsync(ContabilidadAPIConstans.ORIGEN_CUENTA);

            // Await for all tasks to complete
            await Task.WhenAll(cuentasContablesTask, tiposDeCuentaTask, origenCuentaTask);

            // Retrieve the responses
            var cuentasContablesResponse = await cuentasContablesTask;
            var tiposDeCuentaResponse = await tiposDeCuentaTask;
            var origenCuentaResponse = await origenCuentaTask;

            // Read response bodies if successful
            var cuentasContablesResponseBody = cuentasContablesResponse.IsSuccessStatusCode ? await cuentasContablesResponse.Content.ReadAsStringAsync() : "";
            var tiposDeCuentaResponseBody = tiposDeCuentaResponse.IsSuccessStatusCode ? await tiposDeCuentaResponse.Content.ReadAsStringAsync() : "";
            var origenCuentaResponseBody = origenCuentaResponse.IsSuccessStatusCode ? await origenCuentaResponse.Content.ReadAsStringAsync() : "";

            var cuentasContables = JsonConvert.DeserializeObject<CuentaContableResponse>(cuentasContablesResponseBody);
            var tiposDeCuentaContables = JsonConvert.DeserializeObject<TipoCuentaReponse>(tiposDeCuentaResponseBody);
            var origenCuenta = JsonConvert.DeserializeObject<OrigenCuentaReponse>(origenCuentaResponseBody);

            var origenCuentaDto = cuentasContables.cuentasContables.Select(x =>
                new OrigenCuentaDto
                {
                    cuentaContableId = x.id,
                    origenCuentaId =  origenCuenta.cuentasContables.FirstOrDefault(o => o.id == tiposDeCuentaContables?.cuentasContables.FirstOrDefault(y => y.id == x.tipo).origen).id
                }
            ).ToList();

            foreach (var transaccion in pago.Transacciones)
            {
                transaccion.TipoMovimiento = origenCuentaDto.Find(x => x.cuentaContableId == transaccion.Cuenta).origenCuentaId;
            }

            var dto = new AsientoContableDto(pago);

            string jsonContent = JsonConvert.SerializeObject(dto);
            // Create the content you want to send in the request (if needed)
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            // Create an instance of HttpRequestMessage specifying the HTTP method (POST), URI, and content
            var request = new HttpRequestMessage(HttpMethod.Post, ContabilidadAPIConstans.ASIENTO_CONTABLE);
            request.Content = content;

            // Send the request and await the response
            HttpResponseMessage response = await client.SendAsync(request);

            return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pagos == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descripcion,Estado")] Pago pago)
        {
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pagos == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pagos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pagos'  is null.");
            }
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
          return (_context.Pagos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
