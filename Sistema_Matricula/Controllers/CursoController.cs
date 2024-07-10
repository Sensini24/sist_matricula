using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        [HttpGet]
        public ActionResult AgregarCursoSeccion()
        {
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            ViewBag.Grados =  new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Seccion = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarCursoSeccion(CursoSeccion cursoSeccion)
        {
            if (cursoSeccion != null)
            {
                ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
                ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
                ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
                ViewBag.Seccion = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();

                db.CursoSeccions.Add(cursoSeccion);
                db.SaveChanges();
                return RedirectToAction("ListarCurso", "Curso");
            }
            else
            {
                return View(cursoSeccion);
            }
        }


        // POST: CursoController/Create
        [HttpGet]
        public ActionResult EditarCurso(int id)
        {
            if (ModelState.IsValid)
            {
                Curso curso = db.Cursos.Where(e => e.IdCurso == id).FirstOrDefault();

                return View(curso);
            }

            return RedirectToAction("ListarCurso", "Curso");

        }

        // GET: CursoController/Edit/5
        [HttpPost]
        public ActionResult EditarCurso(Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Update(curso);
                db.SaveChanges();
                return RedirectToAction("ListarCurso", "Curso");
            }
            return View(curso);
        }


        [HttpGet]
        public ActionResult BuscarGradoPorNivel(int idNivel)
        {
            var grados = db.Grados.Where(e => e.IdNivel == idNivel).ToList();
            return Json(grados);
        }

        [HttpGet]
        public ActionResult BuscarSeccionPorGrado(int idGrado)
        {
            var secciones = db.Seccions.Where(e => e.IdGrado == idGrado).ToList();
            return Json(secciones);
        }

    }
}
