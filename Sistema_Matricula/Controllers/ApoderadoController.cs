using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class ApoderadoController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public ApoderadoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        // GET: ApoderadoController
        public ActionResult ListarApoderado()
        {
            var apoderados = db.Apoderados.ToList();
            return View(apoderados);
        }

        public IActionResult ListarApoderadoPartial()
        {
            var apoderados = db.Apoderados.ToList();
            return PartialView("_apoderado", apoderados);
        }

        public ActionResult AgregarApoderado()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarApoderado(Apoderado apoderado)
        {
            if (!ModelState.IsValid)
            {
                return View(apoderado);
            }
            db.Apoderados.Add(apoderado);
            db.SaveChanges();
            return RedirectToAction("ListarApoderado");
        }

        [HttpGet]
        public ActionResult EditarApoderado(int id)
        {
            var apoderado = db.Apoderados.Find(id);
            return View(apoderado);
        }

        [HttpPost]
        public ActionResult EditarApoderado(Apoderado apoderado)
        {
            if (!ModelState.IsValid)
            {
                return View(apoderado);
            }
            db.Apoderados.Update(apoderado);
            db.SaveChanges();
            return RedirectToAction("ListarApoderado");
        }
    }
}
