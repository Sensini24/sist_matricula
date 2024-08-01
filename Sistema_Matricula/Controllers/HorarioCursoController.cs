using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;
using System.Transactions;

namespace Sistema_Matricula.Controllers
{
    public class HorarioCursoController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public HorarioCursoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult ObtenerHorario(int idSeccion)
        {
            var resultado = db.Horarios
                .Join(db.HorarioCursoSeccions, h => h.IdHorario, hcs => hcs.IdHorario, (h, hcs) => new { h, hcs })
                .Join(db.CursoSeccions, hhcs => hhcs.hcs.IdCursoSeccion, cs => cs.IdCursoSeccion, (hhcs, cs) => new { hhcs.h, hhcs.hcs, cs })
                .Join(db.Cursos, hhcs_cs => hhcs_cs.cs.IdCurso, c => c.IdCurso, (hhcs_cs, c) => new { hhcs_cs.h, hhcs_cs.hcs, hhcs_cs.cs, c })
                .Join(db.Docentes, hhcs_cs_c => hhcs_cs_c.cs.IdDocente, d => d.IdDocente, (hhcs_cs_c, d) => new
                {
                    hhcs_cs_c.h,
                    hhcs_cs_c.hcs,
                    hhcs_cs_c.cs,
                    hhcs_cs_c.c,
                    d
                })
                .Where(x => x.cs.IdSeccion == idSeccion)
                .AsEnumerable() // Convertir a enumerable para aplicar métodos locales
                .Select(x => new
                {
                    id = x.h.IdHorario,
                    title = x.c.Nombre + " - " + x.d.Nombre,
                    horainicio= x.h.HoraInicio,
                    horafin = x.h.HoraFin,
                    start = ConvertirAFechaHora(x.h.DiaSemana, x.h.HoraInicio.ToString()),
                    end = ConvertirAFechaHora(x.h.DiaSemana, x.h.HoraFin.ToString()),
                    curso = x.c.Nombre,
                    docente = x.d.Nombre,
                    idcurso = x.c.IdCurso,
                    iddocente = x.d.IdDocente,
                    idhorario = x.h.IdHorario,
                    idhorariocursoseccion = x.hcs.IdHorarioCursoSeccion,
                    rrule = new
                    {
                        freq = "weekly",
                        interval = 5,
                        byweekday = GetWeekdayAbbreviation(x.h.DiaSemana),
                        dtstart = ConvertirAFechaHora(x.h.DiaSemana, x.h.HoraInicio.ToString()), // Fechas de inicio para la recurrencia
                        until = DateTime.Now.AddMonths(12).ToString("2024-12-25") // Ajusta la fecha de finalización según sea necesario
                    }
                })
                .ToList();

            return Json(resultado);
        }


        private static string GetWeekdayAbbreviation(string dayOfWeek)
        {
            return dayOfWeek.ToLower() switch
            {
                "lunes" => "MO",
                "martes" => "TU",
                "miércoles" => "WE",
                "jueves" => "TH",
                "viernes" => "FR",
                "sábado" => "SA",
                "domingo" => "SU",
                _ => throw new ArgumentException("Invalid day of week")
            };
        }

        // Método auxiliar para convertir día y hora a formato de fecha y hora
        private static string ConvertirAFechaHora(string diaSemana, string hora)
        {
            var fechaBase = DateTime.Now.Date;

            // Mapeo de días en español a DayOfWeek
            var mapaDias = new Dictionary<string, DayOfWeek>
            {
                {"Lunes", DayOfWeek.Monday},
                {"Martes", DayOfWeek.Tuesday},
                {"Miércoles", DayOfWeek.Wednesday},
                {"Jueves", DayOfWeek.Thursday},
                {"Viernes", DayOfWeek.Friday},
                {"Sábado", DayOfWeek.Saturday},
                {"Domingo", DayOfWeek.Sunday}
            };

            if (!mapaDias.TryGetValue(diaSemana, out DayOfWeek diaEnum))
            {
                throw new ArgumentException($"Día de la semana no reconocido: {diaSemana}");
            }

            var diasHastaElDia = diaEnum - fechaBase.DayOfWeek;
            if (diasHastaElDia < 0) diasHastaElDia += 7;
            var fecha = fechaBase.AddDays(diasHastaElDia);

            if (TimeSpan.TryParse(hora, out TimeSpan tiempoHora))
            {
                return fecha.Add(tiempoHora).ToString("yyyy-MM-ddTHH:mm:ss") ;
            }

            throw new FormatException($"El formato de la hora '{hora}' no es válido.");
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
                "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" 
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
                        bool sinconflictoseccion = ValidarConflictosHorarioSeccion(idseccion, model.DiaSemana, model.HoraInicio, model.HoraFin);
                        bool sinconflictodocente = ValidarConflictosHorarioDocente(iddocente, model.DiaSemana, model.HoraInicio, model.HoraFin);
                        if (sinconflictoseccion && sinconflictodocente )
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
                        else if(!sinconflictoseccion)
                        {
                            // Maneja el conflicto de horario, por ejemplo, mostrando un mensaje de error
                            TempData["Error"] = "Un curso ya fue asignado a este horario a esta seccion";
                            return RedirectToAction("ListarSeccionesyCursos", "Curso");
                        }
                        else if(!sinconflictodocente)
                        {
                            // Maneja el conflicto de horario, por ejemplo, mostrando un mensaje de error
                            TempData["Error"] = "El docente que dicta el curso ya fue asignado " +
                                "a otra seccion. Cambie el docente por otro que dicte el mismo curso, si es que existe la posibilidad";
                            return RedirectToAction("ListarSeccionesyCursos", "Curso");
                        }

                        
                    }

                    db.HorarioCursoSeccions.Add(horarioCursoSeccion);
                    db.SaveChanges();

                    transaction.Commit();

                    return RedirectToAction("ListarSeccionesyCursos", "Curso");
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

        public bool ValidarConflictosHorarioSeccion(int idSeccion, string diaSemana, TimeSpan horaInicio, TimeSpan horaFin)
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
            // Retorna true si no hay conflictos en ninguno de los casos
            return !conflictoSeccion;
        }
        public bool ValidarConflictosHorarioDocente(int? idDocente, string diaSemana, TimeSpan horaInicio, TimeSpan horaFin)
        {
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
            return !conflictoDocente;
        }

        [HttpDelete]
        public IActionResult EliminarHorarioCursoSeccion(int idhcs, int idhorario)
        {
            var horario = db.Horarios.Where(e => e.IdHorario == idhorario).FirstOrDefault();
            var horarioCursoSeccion = db.HorarioCursoSeccions.Where(e => e.IdHorarioCursoSeccion == idhcs).FirstOrDefault();
            var curso = from cs in db.CursoSeccions
                        join c in db.Cursos on cs.IdCurso equals c.IdCurso
                        where cs.IdCursoSeccion == horarioCursoSeccion.IdCursoSeccion
                        select c.Nombre;

            if(horario != null && horarioCursoSeccion != null)
            {
                using(var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.HorarioCursoSeccions.Remove(horarioCursoSeccion);
                        db.Horarios.Remove(horario);
                        db.SaveChanges();
                        TempData["ExitoEliminacionHorario"] = $"Se Elimino el horario asignado al curso " +
                            $"{curso}";
                        transaction.Commit();
                        return RedirectToAction("Asignar");
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        TempData["ErrorEliminacionHorario"] = "No se pudo eliminar el horario del curso";
                        return RedirectToAction("Asignar");
                    }
                }

            }
            else
            {
                return RedirectToAction("Asignar");
            }

            

        }

    }
}
