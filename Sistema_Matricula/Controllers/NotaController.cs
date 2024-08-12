using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class NotaController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public NotaController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public IActionResult PortalNotasCursos()
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;

            var resultado = from cd in db.CursoDocentes
                            join d in db.Docentes on cd.IdDocente equals d.IdDocente
                            join c in db.Cursos on cd.IdCurso equals c.IdCurso
                            where d.IdDocente == docenteid
                            select new CursosYDetallesdeDocente
                            {
                                IdCursoDocente = cd.IdCursoDocente,
                                NombreDocente = d.Nombre,
                                IdDocente = d.IdDocente,
                                NombreCurso = c.Nombre,
                                IdCurso = c.IdCurso
                            };


            return View(resultado);
        }

        public IActionResult CantidadAlumnosporCurso(int idCurso)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;
            var cantidadEstudiantes = (from e in db.Estudiantes
                                       join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                       join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                       join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                       join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                       join d in db.Docentes on cs.IdDocente equals d.IdDocente
                                       where d.IdDocente == docenteid && c.IdCurso == idCurso
                                       select e.IdEstudiante).Count();
            
            return Ok(cantidadEstudiantes);
        }

        public IActionResult ObtenerCursosAsignadosADocente()
        {
            var usuarioID = int.Parse(User.Identity.GetUserId());
            var cursosAsignados = db.CursoDocentes.Where(d => d.IdDocente == usuarioID);

            return Ok(cursosAsignados);
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
