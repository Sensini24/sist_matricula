using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class NotaController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public NotaController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        // GET: NotaController
        public ActionResult ListarNota()
        {
            var notas = db.Nota.ToList();
            return View(notas);
        }

        [HttpGet]
        public ActionResult AgregarNota()
        {
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Estudiantes = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre").ToList();
            ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarNota(Notum notum)
        {
            if (!ModelState.IsValid)
            {
                return View(notum);
            }
            db.Nota.Add(notum);
            db.SaveChanges();

            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Estudiantes = new SelectList(db.Estudiantes, "IdEstudiante", "Nombre").ToList();
            ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion").ToList();
            return RedirectToAction("ListarNota");
        }

        [HttpGet]
        public ActionResult EditarNota(int id)
        {

            var nota = db.Nota.Find(id);
            return View(nota);
        }

        [HttpPost]
        public ActionResult EditarNota(Notum nota)
        {
            if (!ModelState.IsValid)
            {
                return View(nota);
            }
            db.Nota.Update(nota);
            db.SaveChanges();
            return RedirectToAction("ListarNota");
        }

    }
}
