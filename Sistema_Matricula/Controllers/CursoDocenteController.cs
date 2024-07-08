using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Sistema_Matricula.ViewsModels;

namespace Sistema_Matricula.Controllers
{
    public class CursoDocenteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public CursoDocenteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        //public async Task<ActionResult> ListarCursoDocente()
        //{
        //    var docentes = await db.Docentes.ToDictionaryAsync(d => d.IdDocente, d => d.Nombre);
        //    var cursos = await db.Cursos.ToDictionaryAsync(c => c.IdCurso, c => c.Nombre);

        //    var cursoDocentes = await db.CursoDocentes
        //       .Select(cd => new CursoDocenteNombresViewModel
        //       {
        //           IdCursoDocente = cd.IdCursoDocente,
        //           IdDocente = cd.IdDocente,
        //           IdCurso = cd.IdCurso,
        //           DocenteNombre = docentes.GetValueOrDefault(cd.IdDocente, "N/A"),
        //           CursoNombre = cursos.GetValueOrDefault(cd.IdCurso, "N/A")
        //       })
        //       .ToListAsync();

        //    return PartialView("_ListarCursoDocente", cursoDocentes);
        //}
        public async Task<ActionResult> ListarCursoDocente()
        {
            var docentes = await db.Docentes.ToDictionaryAsync(d => d.IdDocente, d => d.Nombre);
            var cursos = await db.Cursos.ToDictionaryAsync(c => c.IdCurso, c => c.Nombre);
            ViewBag.Docentes = docentes;
            ViewBag.Cursos = cursos;

            var cursoDocenteViewModels = await db.CursoDocentes
                .Select(cd => new CursoDocenteViewModel
                {
                    IdCursoDocente = cd.IdCursoDocente,
                    IdDocente = cd.IdDocente,
                    IdCurso = cd.IdCurso,
                    NombreDocente = db.Docentes.Where(d => d.IdDocente == cd.IdDocente).Select(d => d.Nombre).FirstOrDefault(),
                    NombreCurso = db.Cursos.Where(c => c.IdCurso == cd.IdCurso).Select(c => c.Nombre).FirstOrDefault()
                })
                .ToListAsync();
            return PartialView("_ListarCursoDocente", cursoDocenteViewModels);
        }

        public async Task<ActionResult> ListarDocentePorId(int id)
        {
            var especialidad = await db.Especialidads.ToDictionaryAsync(e => e.IdEspecialidad, e => e.Especialidad1);
            ViewBag.Especialidad = especialidad;

            var docente = await db.Docentes.Where(d => d.IdDocente == id).ToListAsync();
            return PartialView("_ListarDocentePorId", docente);
        }

        [HttpGet]
        public ActionResult AgregarCursoDocente()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AgregarCursoDocenteView()
        {
            // Crear un nuevo ViewModel
            var viewModel = new CursoDocenteViewModel();

            // Obtener la lista de Docentes y Cursos para los dropdowns
            ViewBag.Docentes = new SelectList(db.Docentes, "IdDocente", "Nombre");
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre");

            // Pasar el ViewModel vacío a la vista
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AgregarCursoDocenteAsincrono(CursoDocenteViewModel viewModel)
        {
            try
            {
                if(viewModel.IdCurso == 0 || viewModel.IdDocente == 0)
                {
                    return BadRequest("Debe seleccionar un Docente y un Curso");
                }
                if (viewModel != null)
                {
                    var cursoDocente = new CursoDocente
                    {
                        IdDocente = viewModel.IdDocente,
                        IdCurso = viewModel.IdCurso,
                        IdCursoDocente = viewModel.IdCursoDocente
                    };

                    await db.CursoDocentes.AddAsync(cursoDocente);
                    await db.SaveChangesAsync();

                    // Convertir CursoDocente a CursoDocenteViewModel, ya que si no se envia este modelo, no se puede mostrar en la vista
                    var cursoDocenteViewModels = db.CursoDocentes.Select(cd => new CursoDocenteViewModel
                    {
                        IdDocente = cd.IdDocente,
                        IdCurso = cd.IdCurso,
                        IdCursoDocente = cd.IdCursoDocente,
                        NombreDocente = db.Docentes.Where(d => d.IdDocente == cd.IdDocente).Select(d => d.Nombre).FirstOrDefault(),
                        NombreCurso = db.Cursos.Where(c => c.IdCurso == cd.IdCurso).Select(c => c.Nombre).FirstOrDefault()
                    }).ToList();

                    return PartialView("_ListarCursoDocente", cursoDocenteViewModels);
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
                

            ViewBag.Docentes = new SelectList(db.Docentes, "IdDocente", "Nombre");
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre");

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AgregarCursoDocente(CursoDocente cursoDocente)
        {
            if (!ModelState.IsValid)
            {
                // Recopilar errores de validación y agregarlos al ModelState
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errores)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(cursoDocente);
            }

            // Verificar la existencia del IdDocente en la base de datos
            var docente = db.Docentes.Find(cursoDocente.IdDocente);
            if (docente == null)
            {
                ModelState.AddModelError("IdDocente", "El Docente especificado no existe.");
                return View(cursoDocente);
            }

            // Guardar CursoDocente si la validación y la existencia del docente son correctas
            db.CursoDocentes.Add(cursoDocente);
            db.SaveChanges();

            return RedirectToAction("ListarCursoDocente");
        }

        


        public IActionResult EditarCursoDocenteView(int id)
        {
            var cursoDocente = db.CursoDocentes.Find(id);

            var viewModel = new CursoDocenteViewModel
            {
                IdDocente = cursoDocente.IdDocente,
                IdCurso = cursoDocente.IdCurso,
                IdCursoDocente = cursoDocente.IdCursoDocente
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditarCursoDocenteView(CursoDocenteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var cursoDocente = new CursoDocente
            {
                IdDocente = viewModel.IdDocente,
                IdCurso = viewModel.IdCurso,
                IdCursoDocente = viewModel.IdCursoDocente
            };
            db.CursoDocentes.Update(cursoDocente);
            db.SaveChanges();

            return RedirectToAction("ListarCursoDocente");
        }
    }
}
