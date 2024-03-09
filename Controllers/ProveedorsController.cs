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

        
        // GET: Proveedors
        public async Task<IActionResult> Index()
        {
            return _context.Proveedores != null ? 
                          View(await _context.Proveedores.ToListAsync()) :
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
        public IActionResult Create()
        {
            var defaultOption = new SelectListItem() { Value = "", Text = "Seleccione tipo de persona" };
            var selectList = new SelectList(new List<SelectListItem> { defaultOption }.Concat(options.Select(p => new SelectListItem { Value = p.Value.ToString(), Text = p.Text })), "Value", "Text");
            ViewBag.TipoPersona = selectList;

            return View();
        }

        public bool ValidarCedula(string cedula)
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
            var isValidCedula = ValidarCedula(proveedor.DocumentoIdentificador);
            if (ModelState.IsValid && isValidCedula)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!isValidCedula)
            {
                ViewBag.isInvalidCedula = true;
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

            var isValidCedula = ValidarCedula(proveedor.DocumentoIdentificador);

            if (ModelState.IsValid && isValidCedula)
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
                ViewBag.isInvalidCedula = true;
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
