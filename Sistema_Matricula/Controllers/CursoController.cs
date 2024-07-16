using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;
using System.Linq;

namespace Sistema_Matricula.Controllers
{
    //[Route("api/")]
    public class CursoController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public CursoController(DbMatNotaHorarioContext context)
        {
            db = context;
        }
        // GET: CursoController
        public ActionResult ListarCurso()
        { 
            var cursos = db.Cursos.ToList();
            return View(cursos);
        }

        [HttpGet("Curso/BuscarCurso")]
        public async Task<IActionResult> ListarCursosApi()
        {
            var cursos = await db.Cursos.ToListAsync();
            return Ok(cursos);
        }


        [HttpGet]
        public async Task<IActionResult> ListarSeccionesyCursos()
        {
            var cursoSeccionViewModel = await db.CursoSeccions.
                Select(cd => new CursoSeccionViewModel
                {
                    IdSeccion = cd.IdSeccion,
                    NombreSeccion = db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.Nombre).FirstOrDefault(),
                    NombreCurso = db.Cursos.Where(e => e.IdCurso == cd.IdCurso).Select(e => e.Nombre).FirstOrDefault(),
                    NombreDocente = db.Docentes.Where(e => e.IdDocente == cd.IdDocente).Select(e => e.Nombre).FirstOrDefault(),
                    NombreGrado = db.Grados.Where(e => e.IdGrado == db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.IdGrado).FirstOrDefault()).Select(e => e.Descripcion).FirstOrDefault(),
                    NombreNivel = db.Nivels.Where(e => e.IdNivel == db.Grados.Where(e => e.IdGrado == db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.IdGrado).FirstOrDefault()).Select(e => e.IdNivel).FirstOrDefault()).Select(e => e.Descripcion).FirstOrDefault()
                }).ToListAsync();

            return PartialView("_ListarSeccionesYCursos", cursoSeccionViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> ListarSeccionesyCursosConParametros(int? idgrado, int? idnivel, int? idseccion)
        {
            var cursoSeccionQuery = db.CursoSeccions
                .Select(cd => new CursoSeccionViewModel
                {
                    IdSeccion = cd.IdSeccion,
                    NombreSeccion = db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.Nombre).FirstOrDefault(),
                    NombreCurso = db.Cursos.Where(e => e.IdCurso == cd.IdCurso).Select(e => e.Nombre).FirstOrDefault(),
                    NombreDocente = db.Docentes.Where(e => e.IdDocente == cd.IdDocente).Select(e => e.Nombre).FirstOrDefault(),
                    NombreGrado = db.Grados.Where(e => e.IdGrado == db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.IdGrado).FirstOrDefault()).Select(e => e.Descripcion).FirstOrDefault(),
                    NombreNivel = db.Nivels.Where(e => e.IdNivel == db.Grados.Where(e => e.IdGrado == db.Seccions.Where(e => e.IdSeccion == cd.IdSeccion).Select(e => e.IdGrado).FirstOrDefault()).Select(e => e.IdNivel).FirstOrDefault()).Select(e => e.Descripcion).FirstOrDefault()
                });

            if (idgrado.HasValue && idnivel.HasValue && idseccion.HasValue)
            {
                cursoSeccionQuery = cursoSeccionQuery.Where(e => e.IdSeccion == idseccion);
            }
            else if (idnivel.HasValue)
            {
                cursoSeccionQuery = cursoSeccionQuery.Where(e => e.NombreNivel == db.Nivels.FirstOrDefault(n => n.IdNivel == idnivel.Value).Descripcion);
            }
            else if (idgrado.HasValue)
            {
                cursoSeccionQuery = cursoSeccionQuery.Where(e => e.NombreGrado == db.Grados.FirstOrDefault(n => n.IdGrado == idgrado.Value).Descripcion);
            }
            var cursoSeccionViewModel = await cursoSeccionQuery.ToListAsync();
            return PartialView("_ListarSeccionesYCursos", cursoSeccionViewModel);
        }

        public IActionResult ListarCursoPorSeccion(int id)
        {
            var cursos = from c in db.Cursos
                         join cs in db.CursoSeccions
                         on c.IdCurso equals cs.IdCurso
                         join s in db.Seccions
                         on cs.IdSeccion equals s.IdSeccion
                         where s.IdSeccion == id
                         select new
                         {
                             c.IdCurso,
                             c.Nombre
                         };
            return Json(cursos);
                         
        }

        // GET: CursoController/Details/5
        [HttpGet]
        public ActionResult AgregarCurso()
        {
            return View();
        }

        // GET: CursoController/Create
        [HttpPost]
        public async Task<ActionResult> AgregarCurso(Curso curso)
        {
            if (ModelState.IsValid)
            {
                await db.Cursos.AddAsync(curso);
                await db.SaveChangesAsync();
                TempData["Success"] = "Curso agregado correctamente";
                
                return RedirectToAction("AgregarCursoDocenteView", "CursoDocente");
            }
            return View(curso);
        }


        [HttpGet]
        public ActionResult AgregarCursoSeccion()
        {
            ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
            ViewBag.Grados =  new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
            ViewBag.Seccion = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
            ViewBag.Docente = new SelectList(db.Docentes, "IdDocente", "Nombre").ToList();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AgregarCursoSeccion(CursoSeccion cursoSeccion)
        {
            if (cursoSeccion != null)
            {
                if(cursoSeccion.IdCurso == 0 || cursoSeccion.IdDocente == 0 || cursoSeccion.IdDocente == null)
                {
                    TempData["Error"] = "Debe seleccionar un curso y un docente";
                    return RedirectToAction("ListarSeccionesyCursos", "Curso");
                }
                var cursoid = from c in db.Cursos
                              join cs in db.CursoSeccions on c.IdCurso equals cs.IdCurso
                              join s in db.Seccions on cs.IdSeccion equals s.IdSeccion
                              where c.IdCurso == cursoSeccion.IdCurso && s.IdSeccion == cursoSeccion.IdSeccion
                              
                              select new
                              {
                                  c.IdCurso
                              }
                              ;

                var cursoidList = await cursoid.ToListAsync();
                if(cursoSeccion.IdSeccion == 0 || cursoSeccion.IdSeccion == null)
                {
                    TempData["Error"] = "Seleccione la sección al desea asignar el curso y docente";
                    return RedirectToAction("ListarSeccionesyCursos", "Curso");
                }
                if (cursoidList.Count > 0 )
                {
                    TempData["Error"] = "El curso ya se encuentra asignado a esta sección";
                    return RedirectToAction("ListarSeccionesyCursos", "Curso");
                }

                ViewBag.Niveles = new SelectList(db.Nivels, "IdNivel", "Descripcion").ToList();
                ViewBag.Grados = new SelectList(db.Grados, "IdGrado", "Descripcion").ToList();
                ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre").ToList();
                ViewBag.Seccion = new SelectList(db.Seccions, "IdSeccion", "Nombre").ToList();
                ViewBag.Docente = new SelectList(db.Docentes, "IdDocente", "Nombre").ToList();

                await db.CursoSeccions.AddAsync(cursoSeccion);
                await db.SaveChangesAsync();
                TempData["Success"] = $"El curso {db.Cursos.Where(c=>c.IdCurso == cursoSeccion.IdCurso).Select(e=>e.Nombre).FirstOrDefault()} fue " +
                    $"asignado a la {db.Seccions.Where(s=>s.IdSeccion == cursoSeccion.IdSeccion).Select(e=>e.Nombre).FirstOrDefault()}";
                return RedirectToAction("ListarSeccionesyCursos", "Curso");
            }
            else
            {
                return View(cursoSeccion);
            }
        }


        [HttpGet]
        public ActionResult EditarCurso(int id)
        {
            if (ModelState.IsValid)
            {
                Curso curso = db.Cursos.Where(e => e.IdCurso == id).FirstOrDefault();

                return View(curso);
            }

            return RedirectToAction("ListarCurso", "Curso");

        }

        [HttpPost]
        public ActionResult EditarCurso(Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Cursos.Update(curso);
                db.SaveChanges();
                return RedirectToAction("ListarCurso", "Curso");
            }
            return View(curso);
        }


        [HttpGet]
        public ActionResult BuscarGradoPorNivel(int idNivel)
        {

            var grados = db.Grados.Where(e => e.IdNivel == idNivel).ToList();
            return Json(grados);
        }

        [HttpGet]
        public ActionResult BuscarSeccionPorGrado(int idGrado)
        {
            var secciones = db.Seccions.Where(e => e.IdGrado == idGrado).ToList();
            return Json(secciones);
        }

        [HttpGet]
        public ActionResult BuscarDocentePorCurso(int idcurso)
        {
            var docentes = db.Docentes
                        .Join(db.CursoDocentes,
                            d => d.IdDocente,
                            cd => cd.IdDocente,
                            (d, cd) => new { d, cd })
                        .Join(db.Cursos,
                            dc => dc.cd.IdCurso,
                            c => c.IdCurso,
                            (dc, c) => new { dc.d, c })
                        .Where(result => result.c.IdCurso == idcurso)
                        .Select(result => new {
                            IdDocente = result.d.IdDocente,
                            NombreDocente = result.d.Nombre
                        })
                        .Distinct()
                        .ToList();

            return Json(docentes);
        }

    }
}
