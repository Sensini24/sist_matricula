using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class SeccionController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public SeccionController(DbMatNotaHorarioContext context)
        {
            db = context;
        }

        // GET: SeccionController
        public ActionResult ListarSeccion()
        {
            var secciones = db.Seccions.ToList();
            return View(secciones);
        }

        [HttpGet]
        public ActionResult AgregarSeccion()
        {
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarSeccion(Seccion seccion)
        {
            if (ModelState.IsValid)
            {
                db.Seccions.Add(seccion);
                db.SaveChanges();
                return RedirectToAction("ListarSeccion", "Seccion");
            }
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            return View(seccion);
        }


        // GET: SeccionController/Edit/5
        // GET: SeccionController/EditarSeccion/5
        [HttpGet("Seccion/EditarSeccion/{id}")]
        public ActionResult EditarSeccion(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("ListarSeccion", "Seccion");
            }

            Seccion seccion = db.Seccions.FirstOrDefault(x => x.IdSeccion == id);

            if (seccion == null)
            {
                return RedirectToAction("ListarSeccion", "Seccion");
            }

            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            return View(seccion);
        }

        // POST: SeccionController/EditarSeccion
        [HttpPost]
        public ActionResult EditarSeccion(Seccion seccion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
                return View(seccion);
            }

            db.Seccions.Update(seccion);
            db.SaveChanges();

            return RedirectToAction("ListarSeccion");
        }
    }
}
