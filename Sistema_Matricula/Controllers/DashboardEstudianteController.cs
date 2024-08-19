using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;
using Sistema_Matricula.ViewsModels;
using System.Globalization;

namespace Sistema_Matricula.Controllers
{
    public class DashboardEstudianteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public DashboardEstudianteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public int obtenerIdEstudiante()
        {
            var idUsuario = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var idEstudiante = db.Estudiantes.Where(d => d.IdUsuario == idUsuario).FirstOrDefault().IdEstudiante;
            return idEstudiante;
        }

        public IActionResult ObtenerCursosAsignados()
        {
            var resultado =
                            from e in db.Estudiantes
                            join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                            join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                            join g in db.Grados on s.IdGrado equals g.IdGrado
                            join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                            join c in db.Cursos on cs.IdCurso equals c.IdCurso
                            where e.IdEstudiante == obtenerIdEstudiante()
                            select new
                            {
                                Estudiante = e,
                                CursoNombre = c.Nombre
                            };


            ////Para obtener las notas asignadas a un estudiante
            //var cursos =

            //                from c in db.Cursos
            //                join cs in db.CursoSeccions on c.IdCurso equals cs.IdCurso
            //                join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
            //                join nt in db.Nota on c.IdCurso equals nt.IdCurso
            //                join e in db.Estudiantes on nt.IdEstudiante equals e.IdEstudiante
            //                join m in db.Matriculas on  e.IdEstudiante equals m.IdEstudiante
            //                where e.IdEstudiante == obtenerIdEstudiante()
            //                select new
            //                {
            //                    Estudiante = e,
            //                    Matricula = m,
            //                    Seccion = s,
            //                    Curso = c,
            //                    Nota = nt

            //                };

            //var cursos2 = cursos.Where(c => c.Matricula.IdSeccion == c.Seccion.IdSeccion).Select(c => new Curso
            //{
            //    IdCurso = c.Curso.IdCurso,
            //    Nombre = c.Curso.Nombre,
            //}).Distinct();

            return Json(resultado.Distinct());
        }

        public IActionResult ObtenerDocentesAsignados()
        {
            var docentes =(from e in db.Estudiantes
                         join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                         join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                         join g in db.Grados on s.IdGrado equals g.IdGrado
                         join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                         join c in db.Cursos on cs.IdCurso equals c.IdCurso
                         join cd in db.CursoDocentes on cs.IdDocente equals cd.IdDocente
                         join d in db.Docentes on cd.IdDocente equals d.IdDocente
                         where e.IdEstudiante == obtenerIdEstudiante()
                         select new { d.Nombre, d.Apellido }).Distinct().ToList();

            return Json(docentes);
        }

        public IActionResult ObtenerHorariosEstudiante()
        {
            var resultado =
                            from e in db.Estudiantes
                            join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                            join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                            join g in db.Grados on s.IdGrado equals g.IdGrado
                            join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                            join hcs in db.HorarioCursoSeccions on cs.IdCursoSeccion equals hcs.IdCursoSeccion
                            join h in db.Horarios on hcs.IdHorario equals h.IdHorario
                            where e.IdEstudiante == obtenerIdEstudiante()
                            select new
                            {
                                Estudiante = e,
                                Horario = h,
                                CursoSeccion = cs
                            };

            return Json(resultado.Distinct().ToList());
        }

        public DayOfWeek ConvertirDiaADate(string dia)
        {
            switch (dia)
            {
                case "Lunes":
                    return DayOfWeek.Monday;
                case "Martes":
                    return DayOfWeek.Tuesday;
                case "Miércoles":
                    return DayOfWeek.Wednesday;
                case "Jueves":
                    return DayOfWeek.Thursday;
                case "Viernes":
                    return DayOfWeek.Friday;
                case "Sábado":
                    return DayOfWeek.Saturday;
                case "Domingo":
                    return DayOfWeek.Sunday;
                default:
                    throw new ArgumentException("Día no válido");
            }
        }

        public IActionResult ObtenerHorarioCercano()
        {
            string diaActual = DateTime.Now.ToString("dddd", new CultureInfo("es-ES"));
            string[] diasSemana = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
            int indexDiaActual = Array.IndexOf(diasSemana, diaActual);
            string[] diasProximos = {
                diasSemana[(indexDiaActual + 1) % 7],
                diasSemana[(indexDiaActual + 2) % 7]
            };

            var horarios = from e in db.Estudiantes
                           join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                           join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                           join g in db.Grados on s.IdGrado equals g.IdGrado
                           join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                           join d in db.Docentes on cs.IdDocente equals d.IdDocente
                           join c in db.Cursos on cs.IdCurso equals c.IdCurso
                           join hcs in db.HorarioCursoSeccions on cs.IdCursoSeccion equals hcs.IdCursoSeccion
                           join h in db.Horarios on hcs.IdHorario equals h.IdHorario
                           where e.IdEstudiante == obtenerIdEstudiante()
                           where diasProximos.Contains(h.DiaSemana)
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

            return PartialView("_HorarioCercano", horarios.ToList());
        }


    }
}
