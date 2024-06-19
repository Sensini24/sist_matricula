using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class AulaController : Controller
    {

        private readonly DbMatNotaHorarioContext db;

        public AulaController(DbMatNotaHorarioContext _contexto)
        {
            db = _contexto;
        }
        // GET: AulaController
        public ActionResult ListarAula()
        {
            var aulas = db.Aulas.ToList();
            return View(aulas);
        }

        // GET: AulaController/Details/5
        [HttpGet]
        public ActionResult AgregarAula()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AgregarAula(Aula aula)
        {
            if(ModelState.IsValid)
            {
                db.Aulas.Add(aula);
                db.SaveChanges();
                return RedirectToAction("ListarAula");
            }

            return View(aula);
        }


        // GET: AulaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AulaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AulaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AulaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AulaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AulaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
