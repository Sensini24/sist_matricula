using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class HorarioCursoController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public HorarioCursoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public IActionResult ListarCursoHorario()
        {
            var cursoHorarios = db.HorarioCursos.ToList();
            return View(cursoHorarios);
        }

        [HttpGet]
        public ActionResult AgregarCursoHorario()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AgregarCursoHorario(HorarioCurso horarioCurso)
        {
            if (!ModelState.IsValid)
            {
                return View(horarioCurso);
            }
            db.HorarioCursos.Add(horarioCurso);
            db.SaveChanges();

            return RedirectToAction("ListarCursoHorario");
        }

        public IActionResult EditarCursoHorario(int id)
        {
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Horarios = new SelectList(db.Horarios, "IdHorario", "IdHorario").ToList();

            var cursoHorario = db.HorarioCursos.Find(id);
            return View(cursoHorario);
        }

        [HttpPost]
        public IActionResult EditarCursoHorario(HorarioCurso horarioCurso)
        {
            if (!ModelState.IsValid)
            {
                // Manejo de errores específicos
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                //foreach (var error in errors)
                //{
                //    ModelState.AddModelError("", error.ErrorMessage);
                //}
                return View(horarioCurso);
            }
            db.HorarioCursos.Update(horarioCurso);
            db.SaveChanges();

            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Horarios = new SelectList(db.Horarios, "IdHorario", "IdHorario").ToList();
            return RedirectToAction("ListarCursoHorario");
        }

        public IActionResult Asignar()
        {
            List<string> dias = new List<string> { 
                "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado" 
            };
            
            var cursos = db.Cursos.ToList();

            var horarios = db.Horarios.ToList();

            var secciones = db.Seccions.ToList();
            var aulas = db.Aulas.ToList();
            var diasSemanas = dias.ToList();

            var model = new HorarioCursoViewModel();

            ViewBag.Cursos = new SelectList(cursos, "IdCurso", "Nombre");
            ViewBag.Horarios = new SelectList(horarios, "IdHorario", "IdHorario");
            ViewBag.Secciones = new SelectList(secciones, "IdSeccion", "Nombre");
            ViewBag.Aulas = new SelectList(aulas, "IdAula", "IdAula");
            ViewBag.Dias = new SelectList(diasSemanas);
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion");
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion");

            return View(model);
        }

        [HttpPost]
        public IActionResult Asignar(HorarioCursoViewModel model)
        {
            var cursoHorario = new HorarioCurso
            {
                IdCurso = model.IdCurso,
                IdHorario = model.IdHorario
            };

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.IdHorario == 0 || model.IdHorario == null)
                    {
                        var horario = new Horario
                        {
                            HoraInicio = model.HoraInicio,
                            HoraFin = model.HoraFin,
                            IdAula = model.IdAula,
                            IdSeccion = model.IdSeccion,
                            DiaSemana = model.DiaSemana
                        };
                        db.Horarios.Add(horario);
                        db.SaveChanges();
                        /* 
                         * Asignar el IdHorario al cursoHorario porque ya se ha creado un nuevo horario
                         * y ambos deben tener el mismo IdHorario, es decir el IdHorario del nuevo horario creado
                         * en la base de datos, ya que uno es foreing key del otro.
                         */
                        cursoHorario.IdHorario = horario.IdHorario;
                    }

                    db.HorarioCursos.Add(cursoHorario);
                    db.SaveChanges();

                    transaction.Commit();

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "No se pudo asignar el curso al horario.");
                }
            }


            var cursos = db.Cursos.ToList();

            // Obtener la lista de horarios disponibles
            var horarios = db.Horarios.ToList();
            ViewBag.Cursos = new SelectList(cursos, "IdCurso", "Nombre");
            ViewBag.Horarios = new SelectList(horarios, "IdHorario", "IdHorario");
            return View(model);
        }
    }
}
