using DocumentFormat.OpenXml.InkML;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;
using Sistema_Matricula.Utils;

namespace Sistema_Matricula.Controllers
{
    public class AulaController : Controller
    {
        private readonly IValidator<Aula> validadorAula;
        private readonly DbMatNotaHorarioContext db;

        public AulaController(DbMatNotaHorarioContext _contexto, IValidator<Aula> _validadorAula)
        {
            db = _contexto;
            validadorAula = _validadorAula;
        }

        public ActionResult PortalAulas()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListarAulas(int pageNumber = 1, int pageSize = 6)
        {
            IQueryable<Aula> query = db.Aulas.OrderBy(c => c.IdAula);
            var totalaulas = query.Count();

            var aulas = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PagedResult<Aula>
            {
                Items = aulas,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalaulas
            };

            return PartialView("_ListarAula", model);
        }

        // GET: AulaController/Details/5
        [HttpGet]
        public ActionResult AgregarAula()
        {
            var seccionesSinAula = from s in db.Seccions
                                   join a in db.Aulas on s.IdSeccion equals a.IdSeccion into seccionAulas
                                   from sa in seccionAulas.DefaultIfEmpty()
                                   where sa.IdAula == null
                                   select s;

            ViewBag.Niveles = new SelectList(db.Nivels.ToList(), "IdNivel", "Descripcion");
            ViewBag.Secciones = new SelectList(seccionesSinAula.ToList(), "IdSeccion", "Nombre");

            return PartialView("_AgregarAula");
        }

        [HttpPost]
        public ActionResult AgregarAula(Aula aula)
        {
            var validaciones = validadorAula.Validate(aula);
            if (!validaciones.IsValid)
            {
                foreach (var error in validaciones.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                db.Aulas.Add(aula);
                db.SaveChanges();
                TempData["ExitosGuardadoAula"] = "Aula guardada correctamente";
                return RedirectToAction("ListarAulas");
            }

            var seccionesSinAula = from s in db.Seccions
                                   join a in db.Aulas on s.IdSeccion equals a.IdSeccion into seccionAulas
                                   from sa in seccionAulas.DefaultIfEmpty()
                                   where sa.IdAula == null
                                   select s;

            ViewBag.Niveles = new SelectList(db.Nivels.ToList(), "IdNivel", "Descripcion");
            ViewBag.IdSeccion = new SelectList(seccionesSinAula, "IdSeccion", "Nombre");

            var errors = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            return Json(new { success = false, errors = errors });
        }

        public async Task<List<Aula>> AulasPorSeccion(int id){
            var aulas = await db.Aulas.Where(x=>x.IdSeccion == id).ToListAsync();
            return aulas;
        }


        public IActionResult ObtenerGradosPorNivel(int idNivel)
        {
            var grados = db.Grados.Where(x => x.IdNivel == idNivel).ToList();
            return Ok(grados);
        }

        public IActionResult ObtenerSeccionesPorGrado(int idGrado)
        {
            var seccionesSinAula = from s in db.Seccions
                                   join a in db.Aulas on s.IdSeccion equals a.IdSeccion into seccionAulas
                                   from sa in seccionAulas.DefaultIfEmpty()
                                   where sa.IdAula == null && s.IdGrado == idGrado
                                   select s;
            return Ok(seccionesSinAula);
        }
    }
}

