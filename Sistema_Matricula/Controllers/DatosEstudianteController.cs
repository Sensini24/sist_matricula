using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class DatosEstudianteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public DatosEstudianteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public int obtenerIdEstudiante()
        {
            var idUsuario = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var idEstudiante = db.Estudiantes.Where(d => d.IdUsuario == idUsuario).FirstOrDefault().IdEstudiante;
            return idEstudiante;
        }

        public IActionResult HorarioEstudiante()
        {

            return View();
        }

        public IActionResult ObtenerBimestres()
        {
           var bimestres = db.Bimestres.ToList();
            return Json(bimestres);
        }

        public IActionResult ObtenerCursosEstudiante()
        {
            var cursosEstudiante = from e in db.Estudiantes
                                   join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                   join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                   join g in db.Grados on s.IdGrado equals g.IdGrado
                                   join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                   join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                   where e.IdEstudiante == obtenerIdEstudiante()
                                   select new Curso
                                   {
                                       IdCurso = c.IdCurso,
                                       Nombre = c.Nombre
                                   };

            return Json(cursosEstudiante.Distinct().ToList());
        }

        public IActionResult ObtenerDiasSemanaPorCurso()
        {
            var dias = from e in db.Estudiantes
                                join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                join g in db.Grados on s.IdGrado equals g.IdGrado
                                join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                join d in db.Docentes on cs.IdDocente equals d.IdDocente
                                join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                join hcs in db.HorarioCursoSeccions on cs.IdCursoSeccion equals hcs.IdCursoSeccion
                                join h in db.Horarios on hcs.IdHorario equals h.IdHorario
                                where e.IdEstudiante == obtenerIdEstudiante()
                                select h.DiaSemana;

            return Json(dias.Distinct().ToList());

        }

        public IActionResult ObtenerHorarioTabla(int idCurso, string diaSemana)
        {
            var horariosTabla = from e in db.Estudiantes
                                join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                join g in db.Grados on s.IdGrado equals g.IdGrado
                                join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                join d in db.Docentes on cs.IdDocente equals d.IdDocente
                                join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                join hcs in db.HorarioCursoSeccions on cs.IdCursoSeccion equals hcs.IdCursoSeccion
                                join h in db.Horarios on hcs.IdHorario equals h.IdHorario
                                where e.IdEstudiante == obtenerIdEstudiante()
                                select new HorarioEstudianteViewModel
                                {
                                    IdCurso = cs.IdCurso,
                                    CursoNombre = c.Nombre,
                                    IdHorario = h.IdHorario,
                                    Dia = h.DiaSemana,
                                    HoraInicio = h.HoraInicio,
                                    HoraFin = h.HoraFin,
                                    IdDocente = cs.IdDocente,
                                    NombreCompletoDocente = $"{d.Nombre} - {d.Apellido}"
                                };

            if (idCurso != 0 && diaSemana != "" && diaSemana != null)
            {
                horariosTabla = horariosTabla.Where(x=>x.IdCurso == idCurso && x.Dia == diaSemana);
            }else if(idCurso != 0)
            {
                horariosTabla = horariosTabla.Where(x=>x.IdCurso == idCurso);
            }else if(diaSemana != "" && diaSemana != null)
            {
                horariosTabla = horariosTabla.Where(x=>x.Dia == diaSemana);
            }


            return PartialView("_HorarioEstudianteTabla", horariosTabla.ToList());
        }

        [HttpGet]
        public IActionResult ObtenerHorarioEstudiante(int? idCurso = null, string? diaSemana = "")
            {
            var resultado = (from e in db.Estudiantes
                             join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                             join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                             join g in db.Grados on s.IdGrado equals g.IdGrado
                             join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                             join d in db.Docentes on cs.IdDocente equals d.IdDocente
                             join c in db.Cursos on cs.IdCurso equals c.IdCurso
                             join hcs in db.HorarioCursoSeccions on cs.IdCursoSeccion equals hcs.IdCursoSeccion
                             join h in db.Horarios on hcs.IdHorario equals h.IdHorario
                             where e.IdEstudiante == obtenerIdEstudiante()
                             select new
                             {
                                 h,
                                 hcs,
                                 cs,
                                 c,
                                 d,
                                 s,
                                 g,
                                 e
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
                                //nombre completo del docente
                                docenteNombreCompleto = x.d.Nombre + " " + x.d.Apellido,
                                idcurso = x.c.IdCurso,
                                iddocente = x.d.IdDocente,
                                idhorario = x.h.IdHorario,
                                idseccion = x.s.IdSeccion,
                                diaSemana = x.h.DiaSemana,
                                idgrado = x.g.IdGrado,
                                idhorariocursoseccion = x.hcs.IdHorarioCursoSeccion,
                                rrule = $"RRULE:FREQ=WEEKLY;INTERVAL=1;BYDAY={GetWeekdayAbbreviation(x.h.DiaSemana)};DTSTART={DateTime.Now:yyyyMMddTHHmmss};UNTIL={DateTime.Now.AddMonths(12):yyyyMMddTHHmmss}"
                            });

            if (idCurso.HasValue && diaSemana != "" && diaSemana != null)
            {
                resultado = resultado.Where(x => x.idcurso == idCurso && x.diaSemana == diaSemana);
            }

            else if (idCurso.HasValue)
            {
                resultado = resultado.Where(x => x.idcurso == idCurso);
            }
            else if (diaSemana != null && diaSemana != "")
            {
                resultado = resultado.Where(x => x.diaSemana == diaSemana);
            }
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
                return fecha.Add(tiempoHora).ToString("yyyy-MM-ddTHH:mm:ss");
            }

            throw new FormatException($"El formato de la hora '{hora}' no es válido.");
        }



        public IActionResult NotasEstudiante()
        {

           return View();
        }

        public IActionResult ListarNotasEstudiantes(int? idCurso = null , int? idBimestre = null)
        {
            var idSeccion = obtenerSeccionEstudiante();

            var resultados = from e in db.Estudiantes
                             join nt in db.Nota on e.IdEstudiante equals nt.IdEstudiante
                             join b in db.Bimestres on nt.IdBimestre equals b.IdBimestre
                             join c in db.Cursos on nt.IdCurso equals c.IdCurso
                             join cs in db.CursoSeccions on c.IdCurso equals cs.IdCurso
                             where e.IdEstudiante == obtenerIdEstudiante() && cs.IdSeccion == idSeccion
                             select new ListaNotaEstudianteViewModel
                             {
                                 IdCurso = c.IdCurso,
                                 NombreCurso = c.Nombre,
                                 IdBimestre = b.IdBimestre,
                                 NombreBimestre = b.Descripcion,
                                 IdNota = nt.IdNota,
                                 Nota = nt.Nota,
                                 DescripcionNota = nt.Descripcion,
                                 IdEstudiante = e.IdEstudiante
                             };

            if(idBimestre.HasValue && idCurso.HasValue)
            {
                resultados = resultados.Where(x=>x.IdCurso == idCurso && x.IdBimestre == idBimestre);
            }else if(idCurso.HasValue)
            {
                resultados = resultados.Where(x=>x.IdCurso == idCurso);
            }else if(idBimestre.HasValue)
            {
                resultados = resultados.Where(x=>x.IdBimestre == idBimestre);
            }

            return PartialView("_ListarNotasEstudiantes", resultados.ToList());

        }



        public int obtenerSeccionEstudiante()
        {
            var idEstudiante = obtenerIdEstudiante();

            var seccion = (from e in db.Estudiantes
                             join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                             join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                             join g in db.Grados on s.IdGrado equals g.IdGrado
                             join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                             where e.IdEstudiante == idEstudiante
                             select cs.IdSeccion)
                .Distinct().FirstOrDefault();

            return seccion;
        }
    }
}
