using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
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

            //var secciones = (from e in db.Estudiantes
            //                join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
            //                join s in db.Seccions on m.IdSeccion equals s.IdSeccion
            //                join g in db.Grados on s.IdGrado equals g.IdGrado
            //                join n in db.Nivels on g.IdNivel equals n.IdNivel
            //                join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
            //                join c in db.Cursos on cs.IdCurso equals c.IdCurso
            //                join d in db.Docentes on cs.IdDocente equals d.IdDocente
            //                where d.IdDocente == 1008
            //                group new { s.IdGrado, s.Nombre, g.Descripcion, n.Descripcion } by new { s.IdGrado, s.Nombre, g.Descripcion, n.Descripcion } into g
            //                select new
            //                {
            //                    Secciones = g.Key.IdGrado,
            //                    NombreSeccion = g.Key.Nombre,
            //                    DescripcionGrado = g.Key.Descripcion,
            //                    DescripcionNivel = g.Key.Descripcion
            //                }
            //                ).Distinct().ToList();

        }

        public IActionResult CantidadAlumnosporCurso(int idCurso)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;
            var cantidadEstudiantes = ObtenerEstudiantesPorCurso(idCurso).Count();
            
            return Ok(cantidadEstudiantes);
        }

        public IActionResult CantidadSeccionesporCurso(int idCurso)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;
            var cantidadSecciones = ConsultaParaObtenerDatosEstudianteDocente().Where(x=>x.Curso.IdCurso == idCurso).Select(x=>x.Seccion.IdSeccion) .Distinct().Count();

            return Ok(cantidadSecciones);
        }

        public IActionResult ListarEstudiantesPorCursoYSeccion(int idCurso, int idSeccion)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;
            var estudiantes = ObtenerEstudiantesPorCurso(idCurso, idSeccion).ToList();

            return PartialView("_EstudiantesPorCurso", estudiantes);
        }

        public IActionResult ListarSeccionesPorCursoYDocente(int idCurso)
        {
            var secciones = ConsultaParaObtenerDatosEstudianteDocente().Where(x => x.Curso.IdCurso == idCurso)
                            .Select(x => new EstudianteCursoSeccionViewModel
                            {
                                Seccion = x.Seccion,
                                Grado = x.Grado,
                                Nivel = x.Nivel,
                                Curso = x.Curso
                            })
                            .Distinct()
                            .ToList();

            return PartialView("_SeccionesPorCurso", secciones);
        }




        public IQueryable<Estudiante> ObtenerEstudiantesPorCurso(int idCurso, int? idSeccion = null)
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;

            var estudiantesQuery = from e in db.Estudiantes
                                   join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                   join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                   join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                   join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                   join d in db.Docentes on cs.IdDocente equals d.IdDocente
                                   where cs.IdCurso == idCurso && d.IdDocente == docenteid && m.FechMatricula.Year == DateTime.Now.Year
                                   select new { Estudiante = e, CursoSeccion = cs };

            if (idSeccion > 0)
            {
                estudiantesQuery = estudiantesQuery.Where(x => x.CursoSeccion.IdSeccion == idSeccion);
            }

            var estudiantes = estudiantesQuery.Select(x => x.Estudiante);

            return estudiantes;
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
                                   { Estudiante = e, CursoSeccion = cs, Matricula = m, Seccion = s, Curso = c, Docente = d, Grado = g, Nivel = n };


            return estudiantesQuery;
        }

        private int ObtenerIdDocenteActual()
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            return db.Docentes.Where(d => d.IdUsuario == userId).Select(d => d.IdDocente).FirstOrDefault();
        }

        public IActionResult ObtenerEstudiantesPorCursoSeccion(int idCurso)
        {
            var estudiantesQuery = ConsultaParaObtenerDatosEstudianteDocente().Where(c=>c.Curso.IdCurso == idCurso).Select(n=>n.Nivel).Distinct().ToList();

            return Ok(estudiantesQuery);
        }

        public IActionResult ObtenerGradosPorCursoDeDocente(int idCurso, int idNivel)
        {
            var estudiantesQuery = ConsultaParaObtenerDatosEstudianteDocente().Where(c => c.Curso.IdCurso == idCurso && c.Grado.IdNivel == idNivel)
                .Select(n => n.Grado).Distinct().ToList();

            return Ok(estudiantesQuery);
        }

        public IActionResult ObtenerSeccionesPorCursoDeDocente(int idCurso, int idNivel, int idGrado)
        {
            var estudiantesQuery = ConsultaParaObtenerDatosEstudianteDocente().Where(c => c.Curso.IdCurso == idCurso && c.Grado.IdNivel == idNivel && c.Grado.IdGrado == idGrado)
                .Select(n => n.Seccion).Distinct().ToList();

            return Ok(estudiantesQuery);
        }

        public IActionResult ObtenerEstudiantesPorCursoTodosParametro(int idCurso, int? idNivel = null, int? idGrado = null, int? idSeccion = null)
        {
            // Obtén el ID del docente actual
            int docenteid = ObtenerIdDocenteActual();

            // Obtén la consulta base de estudiantes por curso y docente
            var estudiantesQuery = ConsultaParaObtenerDatosEstudianteDocente()
                                   .Where(c => c.Curso.IdCurso == idCurso);

            if (idSeccion.HasValue && idSeccion > 0 && idGrado.HasValue && idGrado > 0 && idNivel.HasValue && idNivel > 0)
            {
                estudiantesQuery = estudiantesQuery.Where(x => x.Seccion.IdSeccion == idSeccion && x.Grado.IdGrado == idGrado);
            }else if (idGrado.HasValue && idGrado > 0 && idNivel.HasValue && idNivel > 0)
            {
                estudiantesQuery = estudiantesQuery.Where(x => x.Grado.IdGrado == idGrado && x.Nivel.IdNivel == idNivel);
            }
            else if (idNivel.HasValue && idNivel > 0)
            {
                estudiantesQuery = estudiantesQuery.Where(x => x.Nivel.IdNivel == idNivel);
            }
            var estudiantes = estudiantesQuery.Select(x => x.Estudiante).Distinct().ToList();

            // Retorna la vista parcial con el modelo "Estudinte"
            return PartialView("_EstudiantesPorCurso", estudiantes);
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

        public IActionResult ListarNotas(int idCurso, int idDocente, int idSeccion, int? idEstudiante = null)
        {
            var notas = from e in db.Estudiantes
                        join nt in db.Nota on e.IdEstudiante equals nt.IdEstudiante
                        join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                        join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                        join g in db.Grados on s.IdGrado equals g.IdGrado
                        join n in db.Nivels on g.IdNivel equals n.IdNivel
                        join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                        join c in db.Cursos on cs.IdCurso equals c.IdCurso
                        join d in db.Docentes on cs.IdDocente equals d.IdDocente
                        where d.IdDocente == idDocente && c.IdCurso == idCurso && s.IdSeccion == idSeccion
                        select new EstudianteCursoSeccionViewModel
                        {
                            Estudiante = e,
                            CursoSeccion = cs,
                            Matricula = m,
                            Seccion = s,
                            Curso = c,
                            Docente = d,
                            Grado = g,
                            Nivel = n,
                            Nota = nt
                        };

            if (idEstudiante.HasValue)
            {
                notas = notas.Where(x => x.Estudiante.IdEstudiante == idEstudiante);
            }

            var notasLista = notas.Select(e => new ListaNotaViewModel
            {
                IdEstudiante = e.Estudiante.IdEstudiante,
                NombreCompleto = $"{e.Estudiante.Apellido}, {e.Estudiante.Nombre}",
                Nota = e.Nota.Nota, // Aquí asumimos que siempre hay una nota
                Descripcion = e.Nota.Descripcion,
                Bimestre = e.Nota.IdBimestreNavigation.Descripcion
            }).ToList();

            return PartialView("_NotasEstudiantes", notasLista);
        }


        [HttpGet]
        public ActionResult AgregarNotaBulk(int idCurso, int idSeccion)
        {
            var estudiantes = ConsultaParaObtenerDatosEstudianteDocente()
                            .Where(x => x.Curso.IdCurso == idCurso && x.Seccion.IdSeccion == idSeccion)
                            .ToList();
            if (!estudiantes.Any())
            {
                TempData["ErrorNotas"] = "No hay estudiantes matriculados en el curso y sección seleccionados";
                return RedirectToAction("PortalNotasCursos");
            }
            var viewModel = new AsignacionNotasViewModel
            {
                IdCurso = idCurso,
                NombreCurso = estudiantes.First().Curso.Nombre,
                IdDocente = estudiantes.First().Docente.IdDocente,
                NombreDocente = estudiantes.First().Docente.Nombre,
                IdSeccion = idSeccion,
                Descripcion = string.Empty,
                EstudiantesNotas = estudiantes.Select(e => new NotaEstudianteViewModel
                {
                    IdEstudiante = e.Estudiante.IdEstudiante,
                    NombreCompleto = $"{e.Estudiante.Apellido}, {e.Estudiante.Nombre}",
                    Nota = 0,
                }).ToList()
            };

            ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AgregarNotaBulk(AsignacionNotasViewModel viewModelAsignacion)
        {
            foreach(var estudianteNota in viewModelAsignacion.EstudiantesNotas)
            {
                if(estudianteNota.Nota < 0 || estudianteNota.Nota > 20)
                {
                    TempData["ErrorNota"] = "La nota debe estar entre 0 y 20";
                    ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
                    return RedirectToAction("AgregarNotaBulk", new { idCurso = viewModelAsignacion.IdCurso, idSeccion = viewModelAsignacion.IdSeccion });
                }
                if (viewModelAsignacion?.Descripcion == null || viewModelAsignacion.Descripcion.Equals(""))
                {
                    TempData["ErrorNota"] = "La descripción es requerida";
                    ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
                    return RedirectToAction("AgregarNotaBulk", new { idCurso = viewModelAsignacion.IdCurso,  idSeccion = viewModelAsignacion.IdSeccion });
                }

                if (viewModelAsignacion.IdBimestre == 0 || viewModelAsignacion.IdBimestre == null)
                {
                    TempData["ErrorNota"] = "Seleccione un bimestre";
                    ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
                    return RedirectToAction("AgregarNotaBulk", new { idCurso = viewModelAsignacion.IdCurso, idSeccion = viewModelAsignacion.IdSeccion });
                }

                var nota = new Notum
                {
                    Nota = estudianteNota.Nota,
                    Descripcion = viewModelAsignacion.Descripcion,
                    IdCurso = viewModelAsignacion.IdCurso,
                    IdEstudiante = estudianteNota.IdEstudiante,
                    IdBimestre = viewModelAsignacion.IdBimestre,
                    IdDocente = viewModelAsignacion.IdDocente
                };

                TempData["SuccessNota"] = "Las notas se han guardado correctamente";
                db.Nota.Add(nota);
            }

            ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
            db.SaveChanges();

            return RedirectToAction("PortalNotasCursos");
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
