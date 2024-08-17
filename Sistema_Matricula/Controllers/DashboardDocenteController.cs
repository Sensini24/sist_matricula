using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class DashboardDocenteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public DashboardDocenteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult CantidadEstudiantes()
        {
            var idUsuario = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var idDocente = db.Docentes.Where(d => d.IdUsuario == idUsuario).FirstOrDefault().IdDocente;

            var estudiantes = from e in db.Estudiantes
                              join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                              join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                              join g in db.Grados on s.IdGrado equals g.IdGrado
                              join n in db.Nivels on g.IdNivel equals n.IdNivel
                              join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                              join c in db.Cursos on cs.IdCurso equals c.IdCurso
                              join d in db.Docentes on cs.IdDocente equals d.IdDocente
                              where d.IdDocente == idDocente
                              select e;

            var cantidadEstudiante = estudiantes.Count();
            return Json(cantidadEstudiante);
        }

        public int obtenerIdDocente()
        {
            var idUsuario = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var idDocente = db.Docentes.Where(d => d.IdUsuario == idUsuario).FirstOrDefault().IdDocente;
            return idDocente;
        }

        public IActionResult CantidadCursos()
        {
            var idUsuario = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var idDocente = db.Docentes.Where(d => d.IdUsuario == idUsuario).FirstOrDefault().IdDocente;

            var cursos = from c in db.Cursos
                         join cs in db.CursoSeccions on c.IdCurso equals cs.IdCurso
                         join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
                         join d in db.Docentes on cs.IdDocente equals d.IdDocente
                         where d.IdDocente == idDocente
                         select c;

            var cantidadCursos = cursos.Distinct().Count();
            return Json(cantidadCursos);
        }


        public IActionResult CantidadSecciones()
        {
            var secciones = ConsultaParaObtenerDatosEstudianteDocente().Select(db => db.Seccion).ToList();

            var cantidadSecciones = secciones.Distinct().Count();
            return Json(cantidadSecciones);
        }

        public IActionResult CantidadDeHorarios()
        {
            var resultado = from h in db.Horarios
                            join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                            join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                            where cs.IdDocente == obtenerIdDocente()
                            select h;

            return Json(resultado.Count());

        }


        public IQueryable<EstudianteCursoSeccionViewModel> ConsultaParaObtenerDatosEstudianteDocente()
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;

            var estudiantesQuery = from e in db.Estudiantes
                                   join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                   join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                   join g in db.Grados on s.IdGrado equals g.IdGrado
                                   join n in db.Nivels on g.IdNivel equals n.IdNivel
                                   join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                   join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                   join d in db.Docentes on cs.IdDocente equals d.IdDocente
                                   where d.IdDocente == docenteid && m.FechMatricula.Year == DateTime.Now.Year
                                   select new EstudianteCursoSeccionViewModel
                                   { Estudiante = e, CursoSeccion = cs, Matricula = m, Seccion = s, Curso = c, Docente = d, Grado = g, Nivel = n, };


            return estudiantesQuery;
        }
    }
}
