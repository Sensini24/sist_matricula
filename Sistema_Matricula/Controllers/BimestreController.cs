using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class BimestreController : Controller
    {
        private readonly DbMatNotaHorarioContext db;

        public BimestreController(DbMatNotaHorarioContext _contexto)
        {
            db = _contexto;
        }
        // GET: BimestreController
        public ActionResult ListarBimestre()
        {
            var bimestres = db.Bimestres.ToList();
            return View(bimestres);
        }

        // GET: BimestreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BimestreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BimestreController/Create
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

        // GET: BimestreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BimestreController/Edit/5
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

        // GET: BimestreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BimestreController/Delete/5
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
