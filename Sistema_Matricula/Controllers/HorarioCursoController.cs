using DocumentFormat.OpenXml.InkML;
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
            var cursoHorarios = db.HorarioCursoSeccions.ToList();
            return View(cursoHorarios);
        }

        public IActionResult ListarCursoSeccionDatos(int id)
        {

            return View();
        }

        [HttpGet]
        public ActionResult AgregarCursoHorario()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AgregarHorarioCursoSeccion(HorarioCursoSeccion horarioCursoSeccion)
        {
            if (!ModelState.IsValid)
            {
                return View(horarioCursoSeccion);
            }
            db.HorarioCursoSeccions.Add(horarioCursoSeccion);
            db.SaveChanges();

            return RedirectToAction("ListarCursoHorario");
        }

        public IActionResult EditarCursoHorario(int id)
        {
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Horarios = new SelectList(db.Horarios, "IdHorario", "IdHorario").ToList();

            var cursoHorario = db.HorarioCursoSeccions.Find(id);
            return View(cursoHorario);
        }

        [HttpPost]
        public IActionResult EditarCursoHorario(HorarioCursoSeccion horarioCursoSeccion)
        {
            if (!ModelState.IsValid)
            {
                // Manejo de errores específicos
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                //foreach (var error in errors)
                //{
                //    ModelState.AddModelError("", error.ErrorMessage);
                //}
                return View(horarioCursoSeccion);
            }
            db.HorarioCursoSeccions.Update(horarioCursoSeccion);
            db.SaveChanges();

            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Horarios = new SelectList(db.Horarios, "IdHorario", "IdHorario").ToList();
            return RedirectToAction("ListarCursoHorario");
        }

        [HttpGet]
        public async Task<IActionResult> ListarCursoSeccion(int? idseccion)
        {
            var cursoSeccionQuery = db.CursoSeccions
                .Select(cd => new CursoSeccionViewModel
                {
                    IdCursoSeccion = cd.IdCursoSeccion,
                    IdSeccion = cd.IdSeccion,
                    NombreSeccion = db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.Nombre).FirstOrDefault(),
                    NombreCurso = db.Cursos.Where(e => e.IdCurso == cd.IdCurso).Select(e => e.Nombre).FirstOrDefault(),
                    NombreDocente = db.Docentes.Where(e => e.IdDocente == cd.IdDocente).Select(e => e.Nombre).FirstOrDefault(),
                    NombreGrado = db.Grados.Where(e => e.IdGrado == db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.IdGrado).FirstOrDefault()).Select(e => e.Descripcion).FirstOrDefault(),
                    NombreNivel = db.Nivels.Where(e => e.IdNivel == db.Grados.Where(e => e.IdGrado == db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.IdGrado).FirstOrDefault()).Select(e => e.IdNivel).FirstOrDefault()).Select(e => e.Descripcion).FirstOrDefault()
                });

            if (idseccion.HasValue)
            {
                cursoSeccionQuery = cursoSeccionQuery.Where(e => e.IdSeccion == idseccion);
            }
            var cursoSeccionViewModel = await cursoSeccionQuery.ToListAsync();
            return Json(cursoSeccionViewModel);
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

            var model = new HorarioCursoSeccionViewModel();

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
        public IActionResult Asignar(HorarioCursoSeccionViewModel model)
        {
            var horarioCursoSeccion = new HorarioCursoSeccion
            {
                IdCursoSeccion = model.IdCursoSeccion, //este sera un campo donde se elige una previa asociacion de curso a una seccion
                IdHorario = model.IdHorario
            };

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (model.IdHorario == 0 || model.IdHorario == null)
                    {
                        var idseccion = db.CursoSeccions.Where(e => e.IdCursoSeccion == model.IdCursoSeccion).Select(e => e.IdSeccion).FirstOrDefault();
                        var iddocente = db.CursoSeccions.Where(e => e.IdCursoSeccion == model.IdCursoSeccion).Select(e => e.IdDocente).FirstOrDefault();
                        bool puedeRegistrar = ValidarConflictosHorario(idseccion, iddocente, model.DiaSemana, model.HoraInicio, model.HoraFin);
                        if (puedeRegistrar)
                        {
                            // Procede con el registro del nuevo horario
                            var horario = new Horario
                            {
                                HoraInicio = model.HoraInicio,
                                HoraFin = model.HoraFin,
                                DiaSemana = model.DiaSemana
                            };
                            db.Horarios.Add(horario);
                            db.SaveChanges();
                            /* 
                             * Asignar el IdHorario al cursoHorario porque ya se ha creado un nuevo horario
                             * y ambos deben tener el mismo IdHorario, es decir el IdHorario del nuevo horario creado
                             * en la base de datos, ya que uno es foreing key del otro.
                             */
                            horarioCursoSeccion.IdHorario = horario.IdHorario;
                        }
                        else
                        {
                            // Maneja el conflicto de horario, por ejemplo, mostrando un mensaje de error
                            TempData["Error"] = "El horario ya esta copado o el docente que dicta el curso ya fue asignado " +
                                "a otra seccion";
                            return RedirectToAction("ListarSeccionesyCursos", "Curso");
                        }

                        
                    }

                    db.HorarioCursoSeccions.Add(horarioCursoSeccion);
                    db.SaveChanges();

                    transaction.Commit();

                    return RedirectToAction("Asignar");
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

        public bool ValidarConflictosHorario(int idSeccion, int? idDocente, string diaSemana, TimeSpan horaInicio, TimeSpan horaFin)
        {
            // Verificar conflicto de horario en la misma sección
            var conflictoSeccion = (from h in db.Horarios
                                    join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                                    join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                                    where cs.IdSeccion == idSeccion
                                    && h.DiaSemana == diaSemana
                                    && h.HoraInicio < horaFin
                                    && h.HoraFin > horaInicio
                                    select h).Any();

            // Verificar conflicto de horario para el mismo docente en otras secciones
            var conflictoDocente = (from h in db.Horarios
                                    join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                                    join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                                    where cs.IdDocente == idDocente
                                    && h.DiaSemana == diaSemana
                                    && h.HoraInicio < horaFin
                                    && h.HoraFin > horaInicio
                                    select h).Any();

            // Retorna true si no hay conflictos en ninguno de los casos
            return !conflictoSeccion && !conflictoDocente;
        }

    }
}
