using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Aula>> AulasPorSeccion(int id){
            var aulas = await db.Aulas.Where(x=>x.IdSeccion == id).ToListAsync();
            return aulas;
        }
    }
}
