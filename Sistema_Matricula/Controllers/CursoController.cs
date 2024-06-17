using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class CursoController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public CursoController(DbMatNotaHorarioContext context)
        {
            db = context;
        }
        // GET: CursoController
        public ActionResult ListarCurso()
        { 
            var cursos = db.Cursos.ToList();
            return View(cursos);
        }

        // GET: CursoController/Details/5
        [HttpGet]
        public ActionResult AgregarCurso()
        {
            return View();
        }

        // GET: CursoController/Create
        [HttpPost]
        public ActionResult AgregarCurso(Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Add(curso);
                db.SaveChanges();
                return RedirectToAction("ListarCurso", "Curso");
            }
            return View(curso);
        }

        // POST: CursoController/Create
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

        // GET: CursoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CursoController/Edit/5
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

        // GET: CursoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CursoController/Delete/5
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
