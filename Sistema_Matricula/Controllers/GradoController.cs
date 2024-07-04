using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarGrado(Grado grado)
        {
            if (!ModelState.IsValid)
            {
                return View(grado);
            }
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();

            db.Grados.Add(grado);
            db.SaveChanges();
            return RedirectToAction("ListarGrado");
        }


        [HttpGet]
        public ActionResult EditarGrado(int id)
        {
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
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
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            db.Grados.Update(grado);
            db.SaveChanges();
            return RedirectToAction("ListarGrado");
        }
    }
}
