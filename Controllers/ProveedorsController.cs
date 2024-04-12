using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaChequesNuevo.Data;
using SistemaChequesNuevo.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SistemaChequesNuevo.Dtos;
using SistemaChequesNuevo.Helpers;

namespace SistemaChequesNuevo.Controllers
{
    [Authorize]
    public class ProveedorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly List<OptionTemplate> options = new List<OptionTemplate>
        {
            new OptionTemplate { Value = 1, Text = "Fisica" },
            new OptionTemplate { Value = 2, Text = "Juridica" },
        };
        public ProveedorsController(ApplicationDbContext context)
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

        private SelectList GetTipoPersona()
        {
            var defaultOption = new SelectListItem() { Value = "", Text = "Seleccione tipo de persona" };
            var selectList = new SelectList(new List<SelectListItem> { defaultOption }.Concat(options.Select(p => new SelectListItem { Value = p.Value.ToString(), Text = p.Text })), "Value", "Text");
            return selectList;
        }

        // GET: Proveedors
        public async Task<IActionResult> Index()
        {
            var data = await _context.Proveedores.ToListAsync();
            var cuentasContables = await GetCuentaContableAsync();

            var cuentasContablesList = cuentasContables.Where(x => !String.IsNullOrEmpty(x.Value)).ToList();

            foreach (var solicitud in data)
            {
                solicitud.CuentaContableDescription = cuentasContablesList.FirstOrDefault(x => Convert.ToInt32(x.Value) == solicitud.CuentaContable).Text;
            }

            return _context.Proveedores != null ?
                          
                          View(data) :
                          Problem("Entity set 'ApplicationDbContext.Proveedores'  is null.");
        }

        // GET: Proveedors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedors/Create

        private bool esUnRNCValido(string rnc)
        {
           
            try
            {
                // Eliminar guiones si están presentes
                rnc = rnc.Replace("-", "");

                // Verificar longitud
                if (rnc.Length != 9)
                {
                    return false;
                }

                // Verificar que solo contenga números
                if (!Regex.IsMatch(rnc, @"^\d+$"))
                {
                    return false;
                }

                // Obtener los dígitos del RNC
                int[] digitos = new int[9];
                for (int i = 0; i < 9; i++)
                {
                    digitos[i] = int.Parse(rnc[i].ToString());
                }

                // Calcular dígito verificador
                int suma = 0;
                int[] pesos = { 7, 9, 8, 6, 5, 4, 3, 2 };
                for (int i = 0; i < 8; i++)
                {
                    suma += digitos[i] * pesos[i];
                }
                int residuo = suma % 11;
                int verificador = residuo == 0 ? 2 : 11 - residuo;

                // Comparar dígito verificador
                return digitos[8] == verificador;
            }
            catch
            {
                return false;
            }

        }

        // GET: Proveedors/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.cuentasContables = await GetCuentaContableAsync();
            ViewBag.TipoPersona = GetTipoPersona();
            return View();
        }

        private bool ValidarCedula(string cedula)
        {
            int digito = 0;
            int digitoVerificador = 0;
            bool resultado = false;
            int[] multiplicadores = { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int producto = 0;
            int suma = 0;
            if (cedula.Contains("-"))
                cedula = cedula.Replace("-", "");
            _ = int.TryParse(cedula.Substring(cedula.Length - 1), out digitoVerificador);
            for (int i = 0; i < (cedula.Length - 1); i++)
            {
                _ = int.TryParse(cedula[i].ToString(), out digito);
                producto = digito * multiplicadores[i];
                if (producto >= 10)
                    producto = (producto / 10) + (producto % 10);
                suma += producto;
            }
            if ((suma + digitoVerificador) % 10 == 0)
                resultado = true;
            return resultado;
        }
        // POST: Proveedors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,TipoPersona,DocumentoIdentificador,Balance,CuentaContable,Estado")] Proveedor proveedor)
        {
            
                var isValidCedula = false;
            bool containsAlphabeticCharacters = Regex.IsMatch(proveedor.DocumentoIdentificador, @"[a-zA-Z]");

            var validationErrorMessage = proveedor.TipoPersona == options[0].Value
                    ? "La cedula es invalida"
                    : "El RNC es invalido";

            isValidCedula = (proveedor.TipoPersona == options[0].Value)
                    ? !containsAlphabeticCharacters && ValidarCedula(proveedor.DocumentoIdentificador)
                    : !containsAlphabeticCharacters && esUnRNCValido(proveedor.DocumentoIdentificador);

            var isNegativeBalance = proveedor.Balance < 0;

            if (ModelState.IsValid && isValidCedula && !isNegativeBalance)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!isValidCedula)
            {
                ViewBag.DocumentoErrorMessage = validationErrorMessage;
                ViewBag.TipoPersona = GetTipoPersona();
                ViewBag.cuentasContables = await GetCuentaContableAsync();
                ViewBag.isInvalidCedula = true;
            }

            if (isNegativeBalance)
            {
                ViewBag.isNegativeBalance = true;
                ViewBag.TipoPersona = GetTipoPersona();
                ViewBag.cuentasContables = await GetCuentaContableAsync();
            }
            return View(proveedor);
        }

        // GET: Proveedors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            ViewBag.TipoPersona = GetTipoPersona();
            ViewBag.cuentasContables = await GetCuentaContableAsync();
            return View(proveedor);
        }

        // POST: Proveedors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,TipoPersona,DocumentoIdentificador,Balance,CuentaContable,Estado")] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return NotFound();
            }

            var isValidCedula = false;
            bool containsAlphabeticCharacters = Regex.IsMatch(proveedor.DocumentoIdentificador, @"[a-zA-Z]");

            var validationErrorMessage = proveedor.TipoPersona == options[0].Value
                ? "La cedula es invalida"
                : "El RNC es invalido";

      
                isValidCedula = (proveedor.TipoPersona == options[0].Value)
                    ? !containsAlphabeticCharacters && ValidarCedula(proveedor.DocumentoIdentificador)
                    : !containsAlphabeticCharacters && esUnRNCValido(proveedor.DocumentoIdentificador);

            var isNegativeBalance = proveedor.Balance < 0;

            if (ModelState.IsValid && isValidCedula && !isNegativeBalance)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.Id))
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

            if (!isValidCedula)
            {
                ViewBag.DocumentoErrorMessage = validationErrorMessage;
                ViewBag.cuentasContables = await GetCuentaContableAsync();
                ViewBag.TipoPersona = GetTipoPersona();
                ViewBag.isInvalidCedula = true;
            }

            if (isNegativeBalance)
            {
                ViewBag.isNegativeBalance = true;
            }
            return View(proveedor);
        }

        // GET: Proveedors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }
            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }
        // POST: Proveedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Proveedores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Proveedores'  is null.");
            }
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProveedorExists(int id)
        {
            return (_context.Proveedores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    public class OptionTemplate
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}