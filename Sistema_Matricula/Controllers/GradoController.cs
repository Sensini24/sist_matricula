using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class GradoController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public GradoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        // GET: GradoController
        public ActionResult ListarGrado()
        {
            var grados = db.Grados.ToList();
            return View(grados);
        }


        public ActionResult AgregarGrado()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarGrado(Grado grado)
        {
            if (!ModelState.IsValid)
            {
                return View(grado);
            }
            db.Grados.Add(grado);
            db.SaveChanges();
            return RedirectToAction("ListarGrado");
        }


        [HttpGet]
        public ActionResult EditarGrado(int id)
        {
            var grado = db.Grados.Find(id);
            return View(grado);
        }

        [HttpPost]
        public ActionResult EditarGrado(Grado grado)
        {
            if (!ModelState.IsValid)
            {
                return View(grado);
            }
            db.Grados.Update(grado);
            db.SaveChanges();
            return RedirectToAction("ListarGrado");
        }
    }
}
