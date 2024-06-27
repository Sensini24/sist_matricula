using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class PagoController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public PagoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public IActionResult ListarPago()
        {
            var pagos = db.Pagos.ToList();
            return View(pagos);
        }

        public ActionResult AgregarPago()
        {

            List<SelectListItem> tiposPagos = new List<SelectListItem>
            {
                new SelectListItem { Value = "Yape", Text = "Yape" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia" },
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta" }
            };

            List<SelectListItem> estadoPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Exitoso", Text = "Exitoso" }
            };

            var matriculas = db.Matriculas.ToList();

            ViewBag.TiposPagos = tiposPagos;
            ViewBag.estadoPago = estadoPago;
            /*
             * Aqui deberia listar el monto segun el id de la matricula
            */
            ViewBag.Matriculas = new SelectList(matriculas, "IdMatricula", "IdMatricula");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarPago(Pago pago)
        {
            var matricula = await db.Matriculas.Where(m => m.IdMatricula == pago.IdMatricula).FirstOrDefaultAsync();
            if (matricula == null)
            {
                ModelState.AddModelError("IdMatricula", "La Matricula especificada no existe.");
                return View(pago);
            }

            if(matricula.Estado == "Pagado")
            {
                ModelState.AddModelError("IdMatricula", "La Matricula ya ha sido pagada.");
                return View(pago);
            }
            matricula.Estado = "Pagado";


            // Guardar Pago si la validación y la existencia de la matricula son correctas
            

            List<SelectListItem> tiposPagos = new List<SelectListItem>
            {
                new SelectListItem { Value = "Yape", Text = "Yape" },
                new SelectListItem { Value = "Transferencia", Text = "Transferencia" },
                new SelectListItem { Value = "Tarjeta", Text = "Tarjeta" }
            };

            List<SelectListItem> estadoPago = new List<SelectListItem>
            {
                new SelectListItem { Value = "Exitoso", Text = "Exitoso" }
            };

            var matriculas = db.Matriculas.ToList();

            ViewBag.TiposPagos = tiposPagos;
            ViewBag.estadoPago = estadoPago;
            /*
             * Aqui deberia listar el monto segun el id de la matricula
            */
            ViewBag.Matriculas = new SelectList(matriculas, "IdMatricula", "IdMatricula");

            db.Pagos.Add(pago);
            db.SaveChangesAsync();

            return RedirectToAction("ListarPago");

        }
    }
}
