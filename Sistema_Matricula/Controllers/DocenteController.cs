using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class DocenteController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public DocenteController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public ActionResult ListarDocente()
        {
            var docentes = db.Docentes.ToList();
            return View(docentes);
        }

        [HttpGet]
        public ActionResult AgregarDocente()
        {
            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AgregarDocente(Docente docente)
        {
            if (!ModelState.IsValid)
            {
                return View(docente);
            }
            db.Docentes.Add(docente);
            db.SaveChanges();

            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            return RedirectToAction("ListarDocente");
        }

        [HttpGet]
        public ActionResult EditarDocente(int id)
        {

            var docente = db.Docentes.Find(id);
            return View(docente);
        }

        [HttpPost]
        public ActionResult EditarDocente(Docente docente)
        {
            if (!ModelState.IsValid)
            {
                return View(docente);
            }
            db.Docentes.Update(docente);
            db.SaveChanges();
            return RedirectToAction("ListarDocente");
        }

        public async Task<IActionResult> EliminarDocente(int id)
        {
            
            if (ModelState.IsValid)
            {
                Docente docente = await db.Docentes.Where(d => d.IdDocente == id).FirstOrDefaultAsync();

                if (docente != null)
                {
                    docente.Estado = "inactivo";
                    db.Docentes.Update(docente);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ListarDocente");
                }
                else
                {
                    return RedirectToAction("ListarDocente");
                }
            }

            return RedirectToAction("ListarDocente");
        }

        [HttpGet]
        public List<Docente> listarDocentes()
        {
            return db.Docentes.ToList();
        }
    }
}
