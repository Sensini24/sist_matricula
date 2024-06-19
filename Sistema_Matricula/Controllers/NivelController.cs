using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class NivelController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public NivelController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        // GET: NivelController
        public ActionResult ListarNivel()
        {
            var nivel = db.Nivels.ToList();
            return View(nivel);
        }

        public ActionResult AgregarNivel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarNivel(Nivel nivel)
        {
            if (!ModelState.IsValid)
            {
                return View(nivel);
            }
            db.Nivels.Add(nivel);
            db.SaveChanges();
            return RedirectToAction("ListarNivel");
        }

        [HttpGet]
        public ActionResult EditarNivel(int id)
        {
            var nivel = db.Nivels.Find(id);
            return View(nivel);
        }

        [HttpPost]
        public ActionResult EditarNivel(Nivel nivel)
        {
            if (!ModelState.IsValid)
            {
                return View(nivel);
            }
            db.Nivels.Update(nivel);
            db.SaveChanges();
            return RedirectToAction("ListarNivel");
        }

        

    }
}
