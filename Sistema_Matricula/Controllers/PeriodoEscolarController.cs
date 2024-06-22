using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class PeriodoEscolarController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public PeriodoEscolarController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult ListarPeriodoEscolar()
        {
            var periodoEscolar = db.PeriodoEscolars.ToList();
            return View(periodoEscolar);
        }

        [HttpGet]
        public ActionResult AgregarPeriodoEscolar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            if (!ModelState.IsValid)
            {
                return View(periodoEscolar);
            }
            db.PeriodoEscolars.Add(periodoEscolar);
            db.SaveChanges();

            return RedirectToAction("ListarPeriodoEscolar");
        }

        [HttpGet]
        public ActionResult EditarPeriodoEscolar(int id)
        {

            var periodoEscolar = db.PeriodoEscolars.Find(id);
            return View(periodoEscolar);
        }

        [HttpPost]
        public ActionResult EditarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            if (!ModelState.IsValid)
            {
                return View(periodoEscolar);
            }
            db.PeriodoEscolars.Update(periodoEscolar);
            db.SaveChanges();
            return RedirectToAction("ListarPeriodoEscolar");
        }

        public async Task<IActionResult> EliminarPeriodoEscolar(int id)
        {

            if (ModelState.IsValid)
            {
                PeriodoEscolar periodoEscolar = await db.PeriodoEscolars.Where(d => d.IdPeriodEscolar == id).FirstOrDefaultAsync();

                if (periodoEscolar != null)
                {
                    periodoEscolar.Estado = "Inactivo";
                    db.PeriodoEscolars.Update(periodoEscolar);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListarPeriodoEscolar");
                }
                else
                {
                    return RedirectToAction("ListarPeriodoEscolar");
                }
            }

            return RedirectToAction("ListarPeriodoEscolar");
        }
    }
}
