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


            ViewBag.Estudiantes = new SelectList(db.Estudiantes, "IdEstudiante", "Apellido").ToList();

            ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion").ToList();

            ViewBag.Docentes = new SelectList(db.Docentes, "IdDocente", "Apellido").ToList();
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
            ViewBag.Estudiantes = new SelectList(db.Estudiantes, "IdEstudiante", "Apellido").ToList();
            ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion").ToList();
            ViewBag.Docentes = new SelectList(db.Docentes, "IdDocente", "Apellido").ToList();
            return RedirectToAction("ListarNota");
        }

        [HttpGet]
        public ActionResult EditarNota(int id)
        {

            Notum nota = db.Nota.Where(x => x.IdNota == id).FirstOrDefault();

            if (nota == null)
            {

                return RedirectToAction("ListarSeccion", "Seccion");
            }
            return View(nota);
        }

        [HttpPost]
        public ActionResult EditarNota(Notum notum)
        {
            if (!ModelState.IsValid)
            {
                return View(notum);
            }

            Notum nota = db.Nota.Where(x => x.IdNota == notum.IdNota).FirstOrDefault();
            nota.IdCurso = notum.IdCurso;
            nota.Nota = notum.Nota;
            nota.Descripcion = notum.Descripcion;
            nota.IdEstudiante = notum.IdEstudiante;
            nota.IdBimestre = notum.IdBimestre;
            

            db.Nota.Update(nota);
            db.SaveChanges();
            return RedirectToAction("ListarNota");
        }

    }
}
