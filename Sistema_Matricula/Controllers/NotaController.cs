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

            //var resultado = (from s in db.Seccions
            //                join g in db.Grados on s.IdGrado equals g.IdGrado
            //                join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion into csGroup
            //                from cs in csGroup.DefaultIfEmpty()
            //                join m in db.Matriculas on s.IdSeccion equals m.IdSeccion into mGroup
            //                from m in mGroup.DefaultIfEmpty()
            //                join e in db.Estudiantes on m.IdEstudiante equals e.IdEstudiante into eGroup
            //                from e in eGroup.DefaultIfEmpty()
            //                join d in db.Docentes on cs.IdDocente equals d.IdDocente
            //                join c in db.Cursos on cs.IdCurso equals c.IdCurso
            //                where c.IdCurso == idCurso
            //                      && d.IdDocente == ObtenerIdDocenteActual()
            //                      && (m == null || m.FechMatricula.Year == DateTime.Now.Year)
            //                select new EstudianteCursoSeccionViewModel
            //                {
            //                    Seccion = s,
            //                    Grado = g,
            //                    CursoSeccion = cs,
            //                    Matricula = m,
            //                    Estudiante = e,
            //                    Docente = d,
            //                    Curso = c
            //                })
            //                .Distinct()
            //                .ToList();


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
                                   where d.IdDocente == docenteid &&  m.FechMatricula.Year == DateTime.Now.Year
                                   select new EstudianteCursoSeccionViewModel
                                   { Estudiante = e, CursoSeccion = cs, Matricula = m, Seccion = s, Curso = c, Docente = d, Grado = g, Nivel = n,  };


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

        public IQueryable<EstudianteNotasViewModel> ConsultaParaObtenerDatosNotasDocente()
        {
            var userId = int.Parse(ObtenerClaimsInfo.GetUserId(User));
            var docenteid = db.Docentes.Where(d => d.IdUsuario == userId).FirstOrDefault().IdDocente;

            var notasQuery = from e in db.Estudiantes
                                   join nt in db.Nota on e.IdEstudiante equals nt.IdEstudiante
                                   join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                   join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                   join g in db.Grados on s.IdGrado equals g.IdGrado
                                   join n in db.Nivels on g.IdNivel equals n.IdNivel
                                   join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                   join c in db.Cursos on cs.IdCurso equals c.IdCurso
                                   join d in db.Docentes on cs.IdDocente equals d.IdDocente
                                   where d.IdDocente == docenteid && m.FechMatricula.Year == DateTime.Now.Year
                                   select new EstudianteNotasViewModel
                                   { Estudiante = e, CursoSeccion = cs, Matricula = m, Seccion = s, Curso = c, Docente = d, Grado = g, Nivel = n, Nota = nt };


            return notasQuery;
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
                    return Json(new { success = false, message = "La nota debe estar entre 0 y 20" });
                }
                if (viewModelAsignacion?.Descripcion == null || viewModelAsignacion.Descripcion.Equals(""))
                {
                    TempData["ErrorNota"] = "La descripción es requerida";
                    ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
                    return Json(new {success = false, message = "La descripción es requerida" }); 
                }

                if (viewModelAsignacion.IdBimestre == 0 || viewModelAsignacion.IdBimestre == null)
                {
                    TempData["ErrorNota"] = "Seleccione un bimestre";
                    ViewBag.Bimestres = new SelectList(db.Bimestres, "IdBimestre", "Descripcion");
                    return Json(new { success = false, message = "Seleccione un bimestre" });
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

            return Json(new { success = true, message = "Notas ingresadas" });
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
        public ActionResult EditarNotaModal(int idNota)
        {
            return ViewComponent("EditarNotaVC", new {idNota});
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
            nota.IdDocente = notum.IdDocente;
            

            db.Nota.Update(nota);
            db.SaveChanges();
            TempData["SuccessNota"] = "La nota se ha actualizado correctamente";
            // pasar un json son succres con el temp data que acabo de crear
            return Json(new { success = true, message = "Nota actualizada correctamente" });
        }

        public IActionResult ListarNotas(int idCurso, int idDocente, int idSeccion, int? idEstudiante = null)
        {
            var estudiantesFiltrados = from e in db.Estudiantes
                                       join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                       join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                       join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                       where cs.IdCurso == idCurso && s.IdSeccion == idSeccion
                                       select e.IdEstudiante;

            var notas = from nt in db.Nota
                        join e in db.Estudiantes on nt.IdEstudiante equals e.IdEstudiante
                        join c in db.Cursos on nt.IdCurso equals c.IdCurso
                        join d in db.Docentes on nt.IdDocente equals d.IdDocente
                        where estudiantesFiltrados.Contains(e.IdEstudiante) && c.IdCurso == idCurso
                        select new
                        {
                            Nota = nt,
                            Estudiante = e
                        };



            if (idEstudiante.HasValue)
            {
                notas = notas.Where(x => x.Estudiante.IdEstudiante == idEstudiante);
            }

            var notasLista = notas.Select(e => new ListaNotaViewModel
            {
                IdEstudiante = e.Estudiante.IdEstudiante,
                NombreCompleto = $"{e.Estudiante.Apellido}, {e.Estudiante.Nombre}",
                Nota = e.Nota.Nota, 
                Descripcion = e.Nota.Descripcion,
                Bimestre = e.Nota.IdBimestreNavigation.Descripcion,
                IdBimestre = e.Nota.IdBimestre,
                IdNota = e.Nota.IdNota
            }).ToList();

            return PartialView("_NotasEstudiantes", notasLista);
        }

        public IActionResult ListarBimestre()
        {
            var bimestres = db.Bimestres.ToList();
            return Ok(bimestres);
        }

        public IActionResult ListarTipoNotasPorBimestre(int idCurso, int idSeccion, int idBimestre)
        {
            var estudiantesFiltrados = from e in db.Estudiantes
                                       join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                       join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                       join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                       where cs.IdCurso == idCurso && s.IdSeccion == idSeccion
                                       select e.IdEstudiante;

            var tipoNotas = from nt in db.Nota
                        join e in db.Estudiantes on nt.IdEstudiante equals e.IdEstudiante
                        join b in db.Bimestres on nt.IdBimestre equals b.IdBimestre
                        join c in db.Cursos on nt.IdCurso equals c.IdCurso
                        join d in db.Docentes on nt.IdDocente equals d.IdDocente
                        where estudiantesFiltrados.Contains(e.IdEstudiante) && c.IdCurso == idCurso && b.IdBimestre == idBimestre
                        select new
                        {
                            TipoNota = nt.Descripcion
                        };
            //var resultado = from e in db.Estudiantes
            //                join nt in db.Nota on e.IdEstudiante equals nt.IdEstudiante
            //                join b in db.Bimestres on nt.IdBimestre equals b.IdBimestre
            //                join c in db.Cursos on nt.IdCurso equals c.IdCurso
            //                join cs in db.CursoSeccions  on c.IdCurso equals cs.IdCurso
            //                where b.IdBimestre == idBimestre && c.IdCurso == idCurso && cs.IdSeccion == idSeccion
            //                select new
            //                {
            //                    TipoNota = nt.Descripcion
            //                };

            return Ok(tipoNotas.Distinct().ToList());
        }

        [HttpGet]
        public IActionResult ListarEstudiantesFiltro(int idCurso, int idSeccion)
        {

            var resultado = from e in db.Estudiantes
                            join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                            join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                            join g in db.Grados on s.IdGrado equals g.IdGrado
                            join n in db.Nivels on g.IdNivel equals n.IdNivel
                            join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                            join c in db.Cursos on cs.IdCurso equals c.IdCurso
                            join d in db.Docentes on cs.IdDocente equals d.IdDocente
                            where c.IdCurso == idCurso && s.IdSeccion == idSeccion && m.FechMatricula.Year == DateTime.Now.Year
                            select new Estudiante
                            {
                                IdEstudiante = e.IdEstudiante,
                                Apellido = e.Apellido,
                                Nombre = e.Nombre,
                            };



            return Ok(resultado.Distinct().ToList());
        }

        public IActionResult FiltrarNotas(int idCurso, int idSeccion, int idBimestre, string? tipoNota = null, int? idEstudiante = null)
        {
            //Primero, obtener los estudiantes matriculados en el curso y sección
            var estudiantesFiltrados = from e in db.Estudiantes
                                       join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante
                                       join s in db.Seccions on m.IdSeccion equals s.IdSeccion
                                       join cs in db.CursoSeccions on s.IdSeccion equals cs.IdSeccion
                                       where cs.IdCurso == idCurso && s.IdSeccion == idSeccion && m.FechMatricula.Year == DateTime.Now.Year
                                       select e.IdEstudiante;

            // Luego, obtener las notas de esos estudiantes
            var resultado = from nt in db.Nota
                            join e in db.Estudiantes on nt.IdEstudiante equals e.IdEstudiante
                            join b in db.Bimestres on nt.IdBimestre equals b.IdBimestre
                            join c in db.Cursos on nt.IdCurso equals c.IdCurso
                            join d in db.Docentes on nt.IdDocente equals d.IdDocente
                            where estudiantesFiltrados.Contains(e.IdEstudiante) && c.IdCurso == idCurso
                            select new ListaNotaViewModel
                            {
                                IdEstudiante = e.IdEstudiante,
                                NombreCompleto = $"{e.Apellido}, {e.Nombre}",
                                Descripcion = nt.Descripcion,
                                Nota = nt.Nota,
                                Bimestre = b.Descripcion,
                                IdBimestre = b.IdBimestre,
                                IdNota = nt.IdNota
                            };

            if (idBimestre > 0 && tipoNota != null && idEstudiante.HasValue)
            {
                resultado = resultado.Where(x => x.Descripcion == tipoNota && x.IdEstudiante == idEstudiante);
            }
            else if (idBimestre > 0 && idEstudiante.HasValue)
            {
                resultado = resultado.Where(x => x.IdBimestre == idBimestre && x.IdEstudiante == idEstudiante);
            }
            else if (idBimestre > 0 && tipoNota != null)
            {
                resultado = resultado.Where(x => x.Descripcion == tipoNota);
            } else if (idEstudiante != null) {

                resultado = resultado.Where(x => x.IdEstudiante == idEstudiante);
            }else if (idBimestre > 0)
            {
                resultado = resultado.Where(x => x.IdBimestre == idBimestre);
            }
            else if (tipoNota != null)
            {

                resultado = resultado.Where(x => x.Descripcion == tipoNota);
            }

            return PartialView("_NotasEstudiantes", resultado.ToList());
        }

    }
}
