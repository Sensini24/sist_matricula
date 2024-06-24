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

        public IActionResult Asignar()
        {
            // Obtener la lista de cursos disponibles
            var cursos = db.Cursos.ToList();

            // Obtener la lista de horarios disponibles
            var horarios = db.Horarios.ToList();

            var model = new HorarioCursoViewModel();

            ViewBag.Cursos = new SelectList(cursos, "IdCurso", "Nombre");
            ViewBag.Horarios = new SelectList(horarios, "IdHorario", "IdHorario");



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
