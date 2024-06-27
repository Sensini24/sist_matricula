using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class HorarioController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public HorarioController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult ListarHorario()
        {
            var horarios = db.Horarios.ToList();
            return View(horarios);
        }

        public IActionResult AgregarHorario()
        {
            List<string> dias = new List<string> {
                "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sábado"
            };

            var secciones = db.Seccions.ToList();
            var aulas = db.Aulas.ToList();
            var diasSemanas = dias.ToList();

            ViewBag.Secciones = new SelectList(secciones, "IdSeccion", "Nombre");
            ViewBag.Aulas = new SelectList(aulas, "IdAula", "IdAula");
            ViewBag.Dias = new SelectList(diasSemanas);

            return View();
        }

        [HttpPost]
        public IActionResult AgregarHorario(Horario horario)
        {
            if(!ModelState.IsValid)
            {
                return View(horario);
            }

            db.Horarios.Add(horario);
            db.SaveChanges();

            return RedirectToAction("ListarHorario");
        }

        public IActionResult EditarHorario(int id)
        {
            var horario = db.Horarios.Find(id);
            return View(horario);
        }

        [HttpPost]
        public IActionResult EditarHorario(Horario horario)
        {
            if(!ModelState.IsValid)
            {
                return View(horario);
            }

            db.Horarios.Update(horario);
            db.SaveChanges();

            return RedirectToAction("ListarHorario");
        }

    }
}
