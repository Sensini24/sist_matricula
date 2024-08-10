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
            var estudiantesSinMatricula = from e in db.Estudiantes
                                          join m in db.Matriculas
                                          on e.IdEstudiante equals m.IdEstudiante into emGroup
                                          from m in emGroup.DefaultIfEmpty()
                                          where m == null
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

            var estudiantesSinMatricula = from e in db.Estudiantes
                                          join m in db.Matriculas
                                          on e.IdEstudiante equals m.IdEstudiante into emGroup
                                          from m in emGroup.DefaultIfEmpty()
                                          where m == null
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

            var estudiantesSinMatricula = from e in db.Estudiantes
                                          join m in db.Matriculas
                                          on e.IdEstudiante equals m.IdEstudiante into emGroup
                                          from m in emGroup.DefaultIfEmpty()
                                          where m == null
                                          select new
                                          {
                                              IdEstudiante = e.IdEstudiante,
                                              NombreCompleto = e.Apellido + " " + e.Nombre,

                                          };


            ViewBag.EstudiantesSinMatricula = new SelectList(estudiantesSinMatricula.ToList(), "IdEstudiante", "NombreCompleto");

            if (!ModelState.IsValid)
            {
                return View(matricula);
            }

            db.Matriculas.Add(matricula);
            TempData["ExitoMatricula"] = "La matricula se ha registrado correctamente";
            db.SaveChanges();

            return RedirectToAction("_ListarMatricula");
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

        
        

    }
}
