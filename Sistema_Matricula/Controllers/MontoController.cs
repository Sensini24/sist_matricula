using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class MontoController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public MontoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult ListarMonto()
        {
            var montos = db.Montos.ToList();
            return View(montos);
        }

        [HttpGet]
        public ActionResult AgregarMonto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarMonto(Monto monto)
        {
            if (!ModelState.IsValid)
            {
                return View(monto);
            }
            db.Montos.Add(monto);
            db.SaveChanges();

            return RedirectToAction("ListarMonto");
        }

        [HttpGet]
        public ActionResult EditarMonto(int id)
        {

            var monto = db.Montos.Find(id);
            return View(monto);
        }

        [HttpPost]
        public ActionResult EditarMonto(Monto monto)
        {
            if (!ModelState.IsValid)
            {
                return View(monto);
            }
            db.Montos.Update(monto);
            db.SaveChanges();
            return RedirectToAction("ListarMonto");
        }


    }
}
