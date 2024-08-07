using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Sistema_Matricula.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData["Nombre"] = User.Identity.Name;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public IActionResult GetSidebar()
        //{
        //    var sidebarModel = GetSidebarItemsForRole(User);
        //    return PartialView("_SidebarPartial", sidebarModel);
        //}

        //private List<SidebarItem> GetSidebarItemsForRole(ClaimsPrincipal user)
        //{
        //    var items = new List<SidebarItem>();

        //    if (user.IsInRole("Admin"))
        //    {
        //        items.Add(new SidebarItem { Name = "Matricula", Link = "/Matricula/ListarMatricula" });
        //    }

        //    if (user.IsInRole("Teacher"))
        //    {
        //        items.Add(new SidebarItem { Name = "Lista Estudiantes", Link = "/Estudiante/ListarEstudiantes" });
        //    }

        //    // Add more items based on roles or common items
        //    items.Add(new SidebarItem { Name = "Cursos", Link = "/Curso/AgregarCursoNoModal" });

        //    return items;
        //}
    }
}
