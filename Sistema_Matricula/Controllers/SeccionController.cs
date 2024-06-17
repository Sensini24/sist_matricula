using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return View(seccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SeccionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SeccionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SeccionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SeccionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
