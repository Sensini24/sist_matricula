using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Sistema_Matricula.Service;
using Stripe;

namespace Sistema_Matricula.Controllers
{
    public class PagoController : Controller
    {
        private readonly PagoService _pagoService;
        private readonly DbMatNotaHorarioContext db;
        private readonly IConfiguration _configuration;

        public PagoController(PagoService pagoService, DbMatNotaHorarioContext _db, IConfiguration configuration)
        {
            _pagoService = pagoService;
            db = _db;
            _configuration = configuration;
        }


        public IActionResult ListarPago()
        {
            var pagos = db.Pagos.ToList();
            return View(pagos);
        }

        public async Task<IActionResult> PagarMatricula(int idMatricula)
        {
            var matricula = await db.Matriculas
                .Where(x => x.IdMatricula == idMatricula && x.Estado == "Pendiente")
                .FirstOrDefaultAsync();

            if (matricula == null)
                return NotFound();

            
            var estudiante = await db.Estudiantes
                .Where(x => x.IdEstudiante == matricula.IdEstudiante)
                .FirstOrDefaultAsync();
            if (estudiante == null)
                return NotFound();


            var monto = await db.Montos
                .Where(x => x.IdMonto == matricula.IdMonto)
                .FirstOrDefaultAsync();

            if(monto == null)
                return NotFound();


            ViewBag.StripePublishableKey = _configuration["Stripe:PublishableKey"];
            ViewBag.EstudianteNombre = estudiante.Nombre;
            ViewBag.EstudianteApellido = estudiante.Apellido;
            ViewBag.Monto = monto.Monto1;
            ViewBag.Descripcion = monto.Descripcion;

            return View(matricula);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarPagoMatricula(int idMatricula, string stripeToken)
        {
            try
            {
                string chargeId = await _pagoService.ProcesarPagoMatricula(idMatricula, stripeToken);
                return Json(new { success = true, chargeId = chargeId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult ConfirmacionPago(string id)
        {
            ViewBag.ChargeId = id;
            return View();
        }


        //public ActionResult AgregarPago()
        //{

        //    List<SelectListItem> tiposPagos = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "Yape", Text = "Yape" },
        //        new SelectListItem { Value = "Transferencia", Text = "Transferencia" },
        //        new SelectListItem { Value = "Tarjeta", Text = "Tarjeta" }
        //    };

        //    List<SelectListItem> estadoPago = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "Exitoso", Text = "Exitoso" }
        //    };

        //    var matriculas = db.Matriculas.ToList();

        //    ViewBag.TiposPagos = tiposPagos;
        //    ViewBag.estadoPago = estadoPago;
        //    /*
        //     * Aqui deberia listar el monto segun el id de la matricula
        //    */
        //    ViewBag.Matriculas = new SelectList(matriculas, "IdMatricula", "IdMatricula");

        //    return View();
        //}



        //[HttpPost]
        //public async Task<ActionResult> AgregarPago(Pago pago)
        //{
        //    var matricula = await db.Matriculas.Where(m => m.IdMatricula == pago.IdMatricula).FirstOrDefaultAsync();
        //    if (matricula == null)
        //    {
        //        ModelState.AddModelError("IdMatricula", "La Matricula especificada no existe.");
        //        return View(pago);
        //    }

        //    if(matricula.Estado == "Pagado")
        //    {
        //        ModelState.AddModelError("IdMatricula", "La Matricula ya ha sido pagada.");
        //        return View(pago);
        //    }
        //    matricula.Estado = "Pagado";


        //    // Guardar Pago si la validación y la existencia de la matricula son correctas


        //    List<SelectListItem> tiposPagos = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "Yape", Text = "Yape" },
        //        new SelectListItem { Value = "Transferencia", Text = "Transferencia" },
        //        new SelectListItem { Value = "Tarjeta", Text = "Tarjeta" }
        //    };

        //    List<SelectListItem> estadoPago = new List<SelectListItem>
        //    {
        //        new SelectListItem { Value = "Exitoso", Text = "Exitoso" }
        //    };

        //    var matriculas = db.Matriculas.ToList();

        //    ViewBag.TiposPagos = tiposPagos;
        //    ViewBag.estadoPago = estadoPago;
        //    /*
        //     * Aqui deberia listar el monto segun el id de la matricula
        //    */
        //    ViewBag.Matriculas = new SelectList(matriculas, "IdMatricula", "IdMatricula");

        //    db.Pagos.Add(pago);
        //    db.SaveChangesAsync();

        //    return RedirectToAction("ListarPago");

        //}
    }
}
