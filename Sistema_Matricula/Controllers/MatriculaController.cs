using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class MatriculaController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public MatriculaController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        
        public ActionResult ListarMatricula()
        {
            var matriculas = db.Matriculas.ToList();
            return View(matriculas);
        }

        [HttpGet]
        public ActionResult AgregarMatricula()
        {
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Pendiente", Text = "Pendiente" },
                new SelectListItem { Value = "Matriculado", Text = "Matriculado" },
                new SelectListItem { Value = "Retirado", Text = "Retirado" }
            };
            ViewBag.estados = estados;

            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            ViewBag.Secciones = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
            ViewBag.Periodos = new SelectList(db.PeriodoEscolars, "IdPeriodEscolar", "Nombre").ToList();
            ViewBag.Estudiantes = new SelectList(db.Estudiantes, "IdEstudiante", "Apellido").ToList();
            ViewBag.Montos = new SelectList(db.Montos, "IdMonto", "Monto1").ToList();
            ViewBag.Aulas = new SelectList(db.Aulas, "IdAula", "Capacidad").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarMatricula(Matricula matricula)
        {
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Pendiente", Text = "Pendiente" },
                new SelectListItem { Value = "Matriculado", Text = "Matriculado" },
                new SelectListItem { Value = "Retirado", Text = "Retirado" }
            };
            ViewBag.estados = estados;
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            ViewBag.Secciones = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
            ViewBag.Periodos = new SelectList(db.PeriodoEscolars, "IdPeriodEscolar", "Nombre").ToList();
            ViewBag.Estudiantes = new SelectList(db.Estudiantes, "IdEstudiante", "Apellido").ToList();
            ViewBag.Montos = new SelectList(db.Montos, "IdMonto", "Monto1").ToList();
            ViewBag.Aulas = new SelectList(db.Aulas, "IdAula", "Capacidad").ToList();


            if (!ModelState.IsValid)
            {
                return View(matricula);
            }
            db.Matriculas.Add(matricula);
            db.SaveChanges();

            return RedirectToAction("ListarMatricula");
        }

        [HttpGet]
        public ActionResult EditarMatricula(int id)
        {

            var matricula = db.Matriculas.Find(id);
            return View(matricula);
        }

        [HttpPost]
        public ActionResult EditarMatricula(Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                return View(matricula);
            }
            db.Matriculas.Update(matricula);
            db.SaveChanges();
            return RedirectToAction("ListarMatricula");
        }

        
    }
}
