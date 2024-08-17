using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Controllers
{
    public class DashboardAdministradorController : Controller
    {
        private readonly DbMatNotaHorarioContext db;
        public DashboardAdministradorController(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
