using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public EspecialidadController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }


        // GET: EspecialidadController
        public ActionResult ListarEspecialidad()
        {
            var especialidades = db.Especialidads.ToList() ;
            return View(especialidades);
        }

        [HttpGet]
        public ActionResult CrearEspecialidad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CrearEspecialidad(Especialidad especialidad)
        {
           if (!ModelState.IsValid)
            {
                return View(especialidad);
            }
            db.Especialidads.Add(especialidad);
            db.SaveChanges();
            return RedirectToAction("ListarEspecialidad");
        }

        [HttpGet]
        public ActionResult EditarEspecialidad(int id)
        { 

            var especialidad = db.Especialidads.Find(id);
            return View(especialidad);
        }

        [HttpPost]
        public ActionResult EditarEspecialidad(Especialidad especialidad)
        {
            if (!ModelState.IsValid)
            {
                return View(especialidad);
            }
            db.Especialidads.Update(especialidad);
            db.SaveChanges();
            return RedirectToAction("ListarEspecialidad");
        }
    }
}
