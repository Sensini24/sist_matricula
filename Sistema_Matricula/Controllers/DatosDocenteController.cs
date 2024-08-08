using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class DatosDocenteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public DatosDocenteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public IActionResult HorarioDocente()
        {
            return View();
        }

        private IQueryable<HorarioDocenteViewModel> ObtenerConsultaBaseHorarios(int idDocente)
        {
            return from h in db.Horarios
                   join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                   join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                   join c in db.Cursos on cs.IdCurso equals c.IdCurso
                   join d in db.Docentes on cs.IdDocente equals d.IdDocente
                   join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
                   join g in db.Grados on s.IdGrado equals g.IdGrado
                   join n in db.Nivels on g.IdNivel equals n.IdNivel
                   where d.IdDocente == idDocente
                   select new HorarioDocenteViewModel
                   {
                       HoraInicio = h.HoraInicio,
                       HoraFin = h.HoraFin,
                       DiaSemana = h.DiaSemana,
                       Curso = c.Nombre,
                       Seccion = s.Nombre,
                       Grado = g.Descripcion,
                       Nivel = n.Descripcion,
                       IdDocente = d.IdDocente,
                       IdCurso = c.IdCurso,
                       IdSeccion = s.IdSeccion,
                       IdGrado = g.IdGrado,
                       IdNivel = n.IdNivel

                   };
        }

        //[HttpGet]
        //public IActionResult ObtenerHorariosDocente()
        //{
        //    var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

        //    var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();

            
        //    var viewModelHorario = (from h in db.Horarios
        //                            join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
        //                            join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
        //                            join c in db.Cursos on cs.IdCurso equals c.IdCurso
        //                            join d in db.Docentes on cs.IdDocente equals d.IdDocente
        //                            join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
        //                            join g in db.Grados on s.IdGrado equals g.IdGrado
        //                            join n in db.Nivels on g.IdNivel equals n.IdNivel
        //                            select new HorarioDocenteViewModel
        //                            {
        //                                HoraInicio = h.HoraInicio,
        //                                HoraFin = h.HoraFin,
        //                                DiaSemana = h.DiaSemana,
        //                                Curso = c.Nombre,
        //                                Seccion = s.Nombre,
        //                                Grado = g.Descripcion,
        //                                Nivel = n.Descripcion,
        //                                IdDocente = d.IdDocente,
        //                            }).
        //                            Where(x => x.IdDocente == obtenerDocente.IdDocente).ToList();

        //    return PartialView("_ObtenerHorariosDocente", viewModelHorario);
        //}

        [HttpGet]
        public IActionResult ObtenerHorariosDocente(int idCurso, int idNivel, int idGrado, int idSeccion)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();

            var viewModelHorario = ObtenerConsultaBaseHorarios(obtenerDocente.IdDocente).ToList();

            if(idCurso != 0 && idSeccion != 0 && idNivel != 0 && idGrado != 0)
            {
                viewModelHorario = viewModelHorario.Where(x=>x.IdCurso == idCurso && x.IdSeccion == idSeccion && x.IdNivel == idNivel && x.IdGrado == idGrado).ToList();
            }

            else if (idCurso == 0 && idSeccion == 0 && idNivel == 0 && idGrado == 0)
            {
                viewModelHorario = viewModelHorario;
            }
            else if (idCurso != null && idCurso != 0 && idNivel != null && idNivel != 0 && idGrado != null && idGrado != 0)
            {
                viewModelHorario = viewModelHorario.Where(x => x.IdCurso == idCurso && x.IdNivel == idNivel).ToList();

            }
            else if (idCurso != null && idCurso != 0 && idNivel != null && idNivel != 0)
            {
                viewModelHorario = viewModelHorario.Where(x => x.IdCurso == idCurso && x.IdNivel == idNivel).ToList();

            }
            else if (idSeccion != null && idSeccion != 0)
            {
                viewModelHorario = viewModelHorario.Where(x => x.IdSeccion == idSeccion).ToList();
            }
            else if (idNivel != null && idNivel != 0)
            {
                viewModelHorario = viewModelHorario.Where(x => x.IdNivel == idNivel).ToList();
            }
            else if (idGrado != null && idGrado != 0)
            {
                viewModelHorario = viewModelHorario.Where(x => x.IdGrado == idGrado).ToList();
            }
            else if (idCurso != null && idCurso !=0)
            {
                viewModelHorario = viewModelHorario.Where(x => x.IdCurso == idCurso).ToList();
            }
            

            return PartialView("_ObtenerHorariosDocente", viewModelHorario);
        }

        [HttpGet]
        public IActionResult ObtenerCursosHorarioDocente()
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();

            var consultaBase = ObtenerConsultaBaseHorarios(obtenerDocente.IdDocente).ToList();

            var cursos = consultaBase.Select(x => new
            {
                idCurso = x.IdCurso,
                nombre = x.Curso
            }).Distinct().ToList();


            return Ok(cursos);
        }

        public IActionResult ObtenerNivelesHorarioDocente(int idCurso)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();

            var consultaBase = ObtenerConsultaBaseHorarios(obtenerDocente.IdDocente).ToList();

            var niveles = consultaBase.Select(x => new
            {
                idNivel = x.IdNivel,
                nombre = x.Nivel,
                idCurso = x.IdCurso
            }).Where(x=>x.idCurso == idCurso).Distinct().ToList();

            return Ok(niveles);
        }

        public IActionResult ObtenerGradosHorarioDocente(int idCurso, int idNivel)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();

            var consultaBase = ObtenerConsultaBaseHorarios(obtenerDocente.IdDocente).ToList();

            var grados = consultaBase.Select(x => new
            {
                idGrado = x.IdGrado,
                nombre = x.Grado,
                idNivel = x.IdNivel,
                idCurso = x.IdCurso,
            }).Where(x=>x.idCurso == idCurso && x.idNivel == idNivel).Distinct().ToList();

            return Ok(grados);
        }

        public IActionResult ObtenerSeccionesHorarioDocente(int idCurso, int idNivel, int idGrado)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();

            var consultaBase = ObtenerConsultaBaseHorarios(obtenerDocente.IdDocente).ToList();

            var secciones = consultaBase.Select(x => new
            {
                idSeccion = x.IdSeccion,
                nombre = x.Seccion,
                idNivel = x.IdNivel,
                idCurso = x.IdCurso,
                idGrado = x.IdGrado
            }).Where(x=>x.idNivel == idNivel && x.idCurso == idCurso && x.idGrado == idGrado).Distinct().ToList();

            return Ok(secciones);
        }


        [HttpGet]
        public IActionResult ObtenerHorariosDocenteCursos()
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.Where(x => x.IdUsuario == userId).FirstOrDefault();


            var cursosDistintos = (from h in db.Horarios
                                   join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                                   join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                                   join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                   where cs.IdDocente == obtenerDocente.IdDocente
                                   select new HorarioDocenteViewModel
                                   {
                                       Curso = c.Nombre
                                   }).Distinct().ToList();

            return PartialView("_CursosDocente", cursosDistintos);

        }
        


        [HttpGet]
        public IActionResult ObtenerHorarioDocente(int idCurso, int idNivel, int idGrado, int idSeccion)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

            var obtenerDocente = db.Docentes.FirstOrDefault(x => x.IdUsuario == userId);

            var resultado = (from h in db.Horarios
                             join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                             join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                             join c in db.Cursos on cs.IdCurso equals c.IdCurso
                             join d in db.Docentes on cs.IdDocente equals d.IdDocente
                             join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
                             join g in db.Grados on s.IdGrado equals g.IdGrado
                             join n in db.Nivels on g.IdNivel equals n.IdNivel
                             where cs.IdDocente == obtenerDocente.IdDocente
                             select new
                             {
                                 h,
                                 hcs,
                                 cs,
                                 c,
                                 d,
                                 s,
                                 g,
                                 n
                             })
                            .Select(x => new
                            {
                                id = x.h.IdHorario,
                                title = x.c.Nombre + " - " + x.d.Nombre,
                                seccion = x.s.Nombre,
                                horainicio = x.h.HoraInicio,
                                horafin = x.h.HoraFin,
                                start = ConvertirAFechaHora(x.h.DiaSemana, x.h.HoraInicio.ToString()),
                                end = ConvertirAFechaHora(x.h.DiaSemana, x.h.HoraFin.ToString()),
                                curso = x.c.Nombre,
                                docente = x.d.Nombre,
                                idcurso = x.c.IdCurso,
                                iddocente = x.d.IdDocente,
                                idhorario = x.h.IdHorario,
                                idseccion = x.s.IdSeccion,
                                idnivel = x.n.IdNivel,
                                idgrado = x.g.IdGrado,
                                idhorariocursoseccion = x.hcs.IdHorarioCursoSeccion,
                                rrule = $"RRULE:FREQ=WEEKLY;INTERVAL=1;BYDAY={GetWeekdayAbbreviation(x.h.DiaSemana)};DTSTART={DateTime.Now:yyyyMMddTHHmmss};UNTIL={DateTime.Now.AddMonths(12):yyyyMMddTHHmmss}"
                            });

            if (idCurso != 0 && idSeccion != 0 && idNivel != 0 && idGrado != 0)
            {
                resultado = resultado.Where(x => x.idcurso == idCurso && x.idseccion == idSeccion && x.idnivel == idNivel && x.idgrado == idGrado);
            }

            else if (idCurso == 0 && idSeccion == 0 && idNivel == 0 && idGrado == 0)
            {
                resultado = resultado;
            }
            else if (idCurso != null && idCurso != 0 && idNivel != null && idNivel != 0 && idGrado != null && idGrado != 0)
            {
                resultado = resultado.Where(x => x.idcurso == idCurso && x.idnivel == idNivel);

            }
            else if (idCurso != null && idCurso != 0 && idNivel != null && idNivel != 0)
            {
                resultado = resultado.Where(x => x.idcurso == idCurso && x.idnivel == idNivel);

            }
            else if (idSeccion != null && idSeccion != 0)
            {
                resultado = resultado.Where(x => x.idseccion == idSeccion);
            }
            else if (idNivel != null && idNivel != 0)
            {
                resultado = resultado.Where(x => x.idnivel == idNivel);
            }
            else if (idGrado != null && idGrado != 0)
            {
                resultado = resultado.Where(x => x.idgrado == idGrado);
            }
            else if (idCurso != null && idCurso != 0)
            {
                resultado = resultado.Where(x => x.idcurso == idCurso);
            }
            return Json(resultado);
        }

        //[HttpGet]
        //public IActionResult ObtenerHorarioDocente()
        //{
        //    var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));

        //    var obtenerDocente = db.Docentes.FirstOrDefault(x => x.IdUsuario == userId);

        //    var resultado = (from h in db.Horarios
        //                     join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
        //                     join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
        //                     join c in db.Cursos on cs.IdCurso equals c.IdCurso
        //                     join d in db.Docentes on cs.IdDocente equals d.IdDocente
        //                     join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
        //                     join g in db.Grados on s.IdGrado equals g.IdGrado
        //                     join n in db.Nivels on g.IdNivel equals n.IdNivel
        //                     where cs.IdDocente == obtenerDocente.IdDocente
        //                     select new
        //                     {
        //                         id = h.IdHorario,
        //                         title = c.Nombre + " - " + d.Nombre,
        //                         start = ConvertirAFechaHora(h.DiaSemana, h.HoraInicio.ToString()),
        //                         end = ConvertirAFechaHora(h.DiaSemana, h.HoraFin.ToString()),
        //                         recurrenceRule = $"FREQ=WEEKLY;BYDAY={GetWeekdayAbbreviation(h.DiaSemana)};UNTIL={DateTime.Now.AddMonths(12).ToString("2024-12-29T11:59:59")}"
        //                     })
        //                    .ToList();

        //    return Json(resultado);
        //}

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
                return fecha.Add(tiempoHora).ToString("yyyy-MM-ddTHH:mm:ss");
            }

            throw new FormatException($"El formato de la hora '{hora}' no es válido.");
        }
    }
}
