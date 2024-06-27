using Microsoft.AspNetCore.Mvc;

namespace Sistema_Matricula.Models
{
    public class ConceptoController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public ConceptoController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult ListarConceptos()
        {
            var conceptos = db.Conceptos.ToList();
            return View(conceptos);
        }

        public IActionResult AgregarConcepto()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AgregarConcepto(Concepto concepto)
        {
            if (!ModelState.IsValid)
            {
                return View(concepto);
            }
            db.Conceptos.Add(concepto);
            db.SaveChanges();

            return RedirectToAction("ListarConceptos");
        }

        public IActionResult EditarConcepto(int id)
        {
            var concepto = db.Conceptos.Find(id);
            return View(concepto);
        }

        [HttpPost]
        public IActionResult EditarConcepto(Concepto concepto)
        {
            if (!ModelState.IsValid)
            {
                return View(concepto);
            }
            db.Conceptos.Update(concepto);
            db.SaveChanges();

            return RedirectToAction("ListarConceptos");
        }
    }
}
