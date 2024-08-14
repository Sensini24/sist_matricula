using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class MatriculaController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public MatriculaController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public ActionResult ListaMatricula()
        {
            return View();
        }

        public ActionResult ListarMatriculaCompleta()
        {
            //if(idEstudiante != null || idEstudiante != 0)
            //{
            //    var matriculas = obtenerDatosMatriculas(idEstudiante);
            //    return PartialView("_ListarMatricula", matriculas);
            //}
            var matriculasviewmodel = db.Matriculas
                .Select(o => new MatriculaViewModel
                {
                    IdMatricula = o.IdMatricula,
                    FechMatricula = o.FechMatricula,
                    IdNivel = o.IdNivel,
                    DescripcionNivel = o.IdNivelNavigation.Descripcion,
                    IdGrado = o.IdGrado,
                    DescripcionGrado = o.IdGradoNavigation.Descripcion,
                    IdSeccion = o.IdSeccion,
                    NombreSeccion = o.IdSeccionNavigation.Nombre,
                    IdPeriodEscolar = o.IdPeriodEscolar,
                    NombrePeriodo = o.IdPeriodEscolarNavigation.Nombre,
                    IdEstudiante = o.IdEstudiante,
                    NombreEstudiante = o.IdEstudianteNavigation.Nombre,
                    ApellidoEstudiante = o.IdEstudianteNavigation.Apellido,
                    IdMonto = o.IdMonto,
                    DescripcionMonto = o.IdMontoNavigation.Descripcion,
                    Monto = o.IdMontoNavigation.Monto1,
                    Estado = o.Estado
                });

            return PartialView("_ListarMatricula", matriculasviewmodel);
        }

        public ActionResult ListarMatricula(string estado)
        {
            //if(idEstudiante != null || idEstudiante != 0)
            //{
            //    var matriculas = obtenerDatosMatriculas(idEstudiante);
            //    return PartialView("_ListarMatricula", matriculas);
            //}

            
            var matriculasviewmodel = db.Matriculas
                .Select(o => new MatriculaViewModel
                {
                    IdMatricula = o.IdMatricula,
                    FechMatricula = o.FechMatricula,
                    IdNivel = o.IdNivel,
                    DescripcionNivel = o.IdNivelNavigation.Descripcion,
                    IdGrado = o.IdGrado,
                    DescripcionGrado = o.IdGradoNavigation.Descripcion,
                    IdSeccion = o.IdSeccion,
                    NombreSeccion = o.IdSeccionNavigation.Nombre,
                    IdPeriodEscolar = o.IdPeriodEscolar,
                    NombrePeriodo = o.IdPeriodEscolarNavigation.Nombre,
                    IdEstudiante = o.IdEstudiante,
                    NombreEstudiante = o.IdEstudianteNavigation.Nombre,
                    ApellidoEstudiante = o.IdEstudianteNavigation.Apellido,
                    IdMonto = o.IdMonto,
                    DescripcionMonto = o.IdMontoNavigation.Descripcion,
                    Monto = o.IdMontoNavigation.Monto1,
                    Estado = o.Estado
                });
            if (string.IsNullOrEmpty(estado))
            {
                matriculasviewmodel = matriculasviewmodel;
            }
            else if (estado.Equals("Pagado"))
            {
                matriculasviewmodel = matriculasviewmodel.Where(e=>e.Estado == "Pagado");
            }else if (estado.Equals("Pendiente"))
            {
                matriculasviewmodel = matriculasviewmodel.Where(e=>e.Estado == "Pendiente");
            }
            return PartialView("_ListarMatricula", matriculasviewmodel);
        }

        [HttpGet]
        public ActionResult EstudiantesSinMatricular()
        {
            var currentYear = DateTime.Now.Year;

            var estudiantesSinMatricula = from e in db.Estudiantes
                                          join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante into emGroup
                                          from m in emGroup.DefaultIfEmpty()
                                          join pe in db.PeriodoEscolars on m.IdPeriodEscolar equals pe.IdPeriodEscolar into dad
                                          from pe in dad.DefaultIfEmpty()
                                          where m == null || pe.FechInicio.Year < currentYear
                                          select e;



            return PartialView("_EstudiantesSinMatricular", estudiantesSinMatricula);
        }

        [HttpGet]
        public ActionResult AgregarMatricula()
        {
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Pendiente", Text = "Pendiente" },
                new SelectListItem { Value = "Matriculado", Text = "Matriculado" },
                new SelectListItem { Value = "Retirado", Text = "Retirado" }
            };
            ViewBag.estados = estados;

            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            ViewBag.Secciones = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
            ViewBag.Periodos = new SelectList(db.PeriodoEscolars, "IdPeriodEscolar", "Nombre").ToList();
            ViewBag.Montos = new SelectList(db.Montos, "IdMonto", "Monto1").ToList();
            ViewBag.Aulas = new SelectList(db.Aulas, "IdAula", "Capacidad").ToList();

            var currentYear = DateTime.Now.Year;
            var estudiantesSinMatricula = from e in db.Estudiantes
                                          join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante into emGroup
                                          from m in emGroup.DefaultIfEmpty()
                                          join pe in db.PeriodoEscolars on m.IdPeriodEscolar equals pe.IdPeriodEscolar into dad
                                          from pe in dad.DefaultIfEmpty()
                                          where m == null || pe.FechInicio.Year < currentYear
                                            select new
                                          {
                                              IdEstudiante = e.IdEstudiante,
                                              NombreCompleto = e.Apellido + " " + e.Nombre,

                                          };


            ViewBag.EstudiantesSinMatricula = new SelectList(estudiantesSinMatricula.ToList(), "IdEstudiante", "NombreCompleto");

            return PartialView("_AgregarMatricula");
        }

        [HttpPost]
        public ActionResult AgregarMatricula(Matricula matricula)
        {
            List<SelectListItem> estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "Pendiente", Text = "Pendiente" },
                new SelectListItem { Value = "Matriculado", Text = "Matriculado" },
                new SelectListItem { Value = "Retirado", Text = "Retirado" }
            };
            ViewBag.estados = estados;
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            ViewBag.Secciones = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
            ViewBag.Periodos = new SelectList(db.PeriodoEscolars, "IdPeriodEscolar", "Nombre").ToList();
            ViewBag.Montos = new SelectList(db.Montos, "IdMonto", "Monto1").ToList();
            ViewBag.Aulas = new SelectList(db.Aulas, "IdAula", "Capacidad").ToList();

            var currentYear = DateTime.Now.Year;
            var estudiantesSinMatricula = from e in db.Estudiantes
                                          join m in db.Matriculas on e.IdEstudiante equals m.IdEstudiante into emGroup
                                          from m in emGroup.DefaultIfEmpty()
                                          join pe in db.PeriodoEscolars on m.IdPeriodEscolar equals pe.IdPeriodEscolar into dad
                                          from pe in dad.DefaultIfEmpty()
                                          where m == null || pe.FechInicio.Year < currentYear
                                          select new
                                          {
                                              IdEstudiante = e.IdEstudiante,
                                              NombreCompleto = e.Apellido + " " + e.Nombre,

                                          };


            ViewBag.EstudiantesSinMatricula = new SelectList(estudiantesSinMatricula.ToList(), "IdEstudiante", "NombreCompleto");

            if (!ModelState.IsValid)
            {
                if (matricula.IdEstudiante == 0 || matricula.IdEstudiante == null)
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar un estudiante";
                    return RedirectToAction("ListarMatriculaCompleta");
                }

                if (matricula.IdNivel == 0 || matricula.IdNivel == null)
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar un nivel";
                    return RedirectToAction("ListarMatriculaCompleta");
                }

                if (matricula.IdGrado == 0 || matricula.IdGrado == null)
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar un grado";
                    return RedirectToAction("ListarMatriculaCompleta");
                }

                if (matricula.IdSeccion == 0 || matricula.IdSeccion == null )
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar una sección";
                    return RedirectToAction("ListarMatriculaCompleta");
                }

                if (matricula.IdPeriodEscolar == 0 || matricula.IdPeriodEscolar == null )
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar un periodo escolar";
                    return RedirectToAction("ListarMatriculaCompleta");
                }

                if (matricula.IdMonto == 0 || matricula.IdMonto == null )
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar un monto";
                    return RedirectToAction("ListarMatriculaCompleta");
                }

                if (matricula.Estado == null || matricula.Estado == null )
                {
                    TempData["ErrorMatricula"] = "Debe seleccionar un estado";
                    return RedirectToAction("ListarMatriculaCompleta");
                }
                TempData["ErrorMatricula"] = "La matricula no se ha registrado correctamente";
                return RedirectToAction("ListarMatriculaCompleta");
            }

            

            var estudiante = db.Estudiantes.Find(matricula.IdEstudiante);
            estudiante.Estado = "Pendiente";
            db.Estudiantes.Update(estudiante);

            db.Matriculas.Add(matricula);
            TempData["ExitoMatricula"] = "La matricula se ha registrado correctamente";
            db.SaveChanges();

            return RedirectToAction("ListarMatriculaCompleta");
        }

        [HttpGet]
        public ActionResult EditarMatricula(int id)
        {

            var matricula = db.Matriculas.Find(id);
            return View(matricula);
        }

        [HttpPost]
        public ActionResult EditarMatricula(Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                return View(matricula);
            }
            db.Matriculas.Update(matricula);
            db.SaveChanges();
            return RedirectToAction("ListarMatricula");
        }

        [HttpGet]
        public IActionResult ObtenerEstudianteDNI(string dni)
        {
            // Obtener periodo escolar actual
            var periodoActual = db.PeriodoEscolars
                .FirstOrDefault(pe => DateTime.Now >= pe.FechInicio && DateTime.Now <= pe.FechFinal);

            if (periodoActual == null)
            {
                return Json(new { error = "No hay un periodo escolar activo." });
            }

            var currentYear = DateTime.Now.Year;

            // Buscar estudiante por DNI
            var estudiante = db.Estudiantes.FirstOrDefault(e => e.Dni == dni);

            if (estudiante == null)
            {
                TempData["ErrorEstudiante"] = "No se encontró un estudiante con el DNI ingresado.";
                return Json(new { error = "No se encontró un estudiante con el DNI ingresado." });
            }

            // Verificar si el estudiante ya está matriculado en el periodo actual
            var matriculaActual = db.Matriculas
                .Any(m => m.IdEstudiante == estudiante.IdEstudiante && m.IdPeriodEscolar == periodoActual.IdPeriodEscolar);

            if (matriculaActual)
            {
                TempData["ErrorEstudiante"] = "El estudiante ya está matriculado en el periodo escolar actual.";
                return Json(new { error = "El estudiante ya está matriculado en el periodo escolar actual." });
            }

            // Determinar si es estudiante antiguo o nuevo
            var ultimaMatricula = db.Matriculas
                .Where(m => m.IdEstudiante == estudiante.IdEstudiante)
                .OrderByDescending(m => m.FechMatricula)
                .FirstOrDefault();

            bool esNuevo = ultimaMatricula == null ||
                           db.PeriodoEscolars.Any(pe => pe.IdPeriodEscolar == ultimaMatricula.IdPeriodEscolar && pe.FechInicio.Year < currentYear);

            // Obtener el monto correspondiente
            var monto = db.Montos.FirstOrDefault(x => x.IdMonto == (esNuevo ? 1 : 2));

            var resultado = new
            {
                Estudiante = new
                {
                    estudiante.IdEstudiante,
                    NombreCompleto = $"{estudiante.Apellido} {estudiante.Nombre}"
                },
                PeriodoEscolar = new
                {
                    periodoActual.IdPeriodEscolar,
                    periodoActual.Nombre
                },
                Monto = new
                {
                    monto.IdMonto,
                    nomCompleto = $"{monto.Descripcion} {monto.Monto1}"
                },
                EsNuevo = esNuevo,
                estado = "Pendiente"
            };

            return Ok(resultado);
        }

    }
}
