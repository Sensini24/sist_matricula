using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
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
        public async Task<ActionResult> ListarCursosPorDocente(int id)
        {
            var cursosdeDocentes = await db.Cursos
                .Where(c => c.CursoDocentes.Any(cd => cd.IdDocente == id))
                .Select(c => new CursoDocenteViewModel
                {
                    IdCurso = c.IdCurso,
                    NombreCurso = c.Nombre,
                    IdCursoDocente = c.CursoDocentes.Where(cd => cd.IdDocente == id).Select(cd => cd.IdCursoDocente).FirstOrDefault()
                }).ToListAsync();

            return PartialView("_ListarCursoPorDocente",cursosdeDocentes);

        }


            public async Task<IActionResult> ListarDocentesYCantidadCursos()
        {
            var docentes = await db.Docentes.ToListAsync();
            var cursos = await db.CursoDocentes.ToListAsync();

            var cursoDocenteCountViewModel = await db.CursoDocentes
            .GroupBy(cd => cd.IdDocente)
            .Select(g => new CursoDocenteCountViewModel
            {
                IdDocente = g.Key,
                NombreDocente = db.Docentes.Where(d => d.IdDocente == g.Key).Select(d => d.Nombre).FirstOrDefault(),
                Cantidad = g.Count()
            })
            .ToListAsync();

            return PartialView("_ListarDocentesYCantidadCursos", cursoDocenteCountViewModel);
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
            // Este es el model que se requiere para la vista de _ListarCursoDocente
            try
            {
                if (viewModel.IdCurso == 0 || viewModel.IdDocente == 0)
                {
                    TempData["Error"] = "Debe seleccionar un Docente y un Curso";

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
                if (viewModel != null)
                {
                    // Verificar si el curso ya existe para el docente
                    var cursoExistente = db.CursoDocentes.Any(cd => cd.IdCurso == viewModel.IdCurso && cd.IdDocente == viewModel.IdDocente);

                    if (!cursoExistente)
                    {
                        var cursoDocente = new CursoDocente
                        {
                            IdDocente = viewModel.IdDocente,
                            IdCurso = viewModel.IdCurso,
                            IdCursoDocente = viewModel.IdCursoDocente
                        };

                        await db.CursoDocentes.AddAsync(cursoDocente);
                        await db.SaveChangesAsync();

                        var cursoDocenteViewModels = db.CursoDocentes.Select(cd => new CursoDocenteViewModel
                        {
                            IdDocente = cd.IdDocente,
                            IdCurso = cd.IdCurso,
                            IdCursoDocente = cd.IdCursoDocente,
                            NombreDocente = db.Docentes.Where(d => d.IdDocente == cd.IdDocente).Select(d => d.Nombre).FirstOrDefault(),
                            NombreCurso = db.Cursos.Where(c => c.IdCurso == cd.IdCurso).Select(c => c.Nombre).FirstOrDefault()
                        }).ToList();

                        TempData["Success"] = $"El curso {cursoDocenteViewModels.Where(d => d.IdDocente == viewModel.IdDocente).Select(d => d.NombreCurso).LastOrDefault()}" +
                            $" ha sido asignado al docente {cursoDocenteViewModels.Where(d => d.IdDocente == viewModel.IdDocente).Select(d => d.NombreDocente).FirstOrDefault()}.";
                        // Se devueleve el partial y con este le model para que se actualice la tabla
                        return RedirectToAction("ListarCursosPorDocente", new { id = cursoDocente.IdDocente });
                    }
                    else
                    {

                        var cursoDocenteViewModels = db.CursoDocentes.Select(cd => new CursoDocenteViewModel
                        {
                            IdDocente = cd.IdDocente,
                            IdCurso = cd.IdCurso,
                            IdCursoDocente = cd.IdCursoDocente,
                            NombreDocente = db.Docentes.Where(d => d.IdDocente == cd.IdDocente).Select(d => d.Nombre).FirstOrDefault(),
                            NombreCurso = db.Cursos.Where(c => c.IdCurso == cd.IdCurso).Select(c => c.Nombre).FirstOrDefault()
                        }).ToList();
                            var curso = from c in db.Cursos
                                        where c.IdCurso == viewModel.IdCurso
                                        select c.Nombre;

                            TempData["Error"] = $"EL curso {cursoDocenteViewModels.Where(d => d.IdCurso == viewModel.IdCurso).Select(d => d.NombreCurso).LastOrDefault()}" +
                                $" ya fue asignado al docente {cursoDocenteViewModels.Where(d => d.IdDocente == viewModel.IdDocente).Select(d => d.NombreDocente).FirstOrDefault()}.";



                        return RedirectToAction("ListarCursosPorDocente", new { id = viewModel.IdDocente });
                    }
                }
            }
            catch (Exception ex)
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


        public async Task<IActionResult> EliminarCursoDocente(int id)
        {
            var cursoDocente = await db.CursoDocentes.FirstOrDefaultAsync(cd => cd.IdCursoDocente == id);

            if (cursoDocente == null)
            {
                return NotFound("El curso asignado no existe.");
            }

            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    var horariosCursoSeccionAEliminar = from hcs in db.HorarioCursoSeccions
                                                        join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                                                        join cd in db.CursoDocentes on cs.IdCurso equals cd.IdCurso
                                                        where cd.IdCursoDocente == cursoDocente.IdCursoDocente
                                                        select hcs;

                    db.HorarioCursoSeccions.RemoveRange(horariosCursoSeccionAEliminar);

                    var horariosAEliminar = from h in db.Horarios
                                            join hcs in db.HorarioCursoSeccions on h.IdHorario equals hcs.IdHorario
                                            join cs in db.CursoSeccions on hcs.IdCursoSeccion equals cs.IdCursoSeccion
                                            join cd in db.CursoDocentes on cs.IdCurso equals cd.IdCurso
                                            where cd.IdCursoDocente == cursoDocente.IdCursoDocente
                                            select h;

                    db.Horarios.RemoveRange(horariosAEliminar);

                    var cursoSeccionesAEliminar = from cs in db.CursoSeccions
                                                  join cd in db.CursoDocentes on cs.IdCurso equals cd.IdCurso
                                                  where cd.IdCursoDocente == cursoDocente.IdCursoDocente
                                                  select cs;

                    db.CursoSeccions.RemoveRange(cursoSeccionesAEliminar);

                    db.CursoDocentes.Remove(cursoDocente);

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync(); 
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ex.Message);
                }
            }

            return RedirectToAction("ListarCursosPorDocente", new { id = cursoDocente.IdDocente });
        }


    }
}
