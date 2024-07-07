using Microsoft.AspNetCore.Mvc;
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

        public async Task<ActionResult> ListarCursoDocente()
        {
            var cursoDocentes = await db.CursoDocentes.ToListAsync();
            return PartialView("_ListarCursoDocente", cursoDocentes);
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
            if (ModelState.IsValid)
            {
                var cursoDocente = new CursoDocente
                {
                    IdDocente = viewModel.IdDocente,
                    IdCurso = viewModel.IdCurso,
                    IdCursoDocente = viewModel.IdCursoDocente
                };

                await db.CursoDocentes.AddAsync(cursoDocente);
                await db.SaveChangesAsync();
                return PartialView("_ListarCursoDocente", db.CursoDocentes.ToList());
            }

            ViewBag.Docentes = new SelectList(db.Docentes, "IdDocente", "Nombre");
            ViewBag.Cursos = new SelectList(db.Cursos, "IdCurso", "Nombre");

            return View(viewModel);
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
