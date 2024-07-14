using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewComponents
{
    public class AgregarCurso : ViewComponent
    {
        private readonly DbMatNotaHorarioContext db;

        public AgregarCurso(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var curso = new Curso();
            return View("_AgregarCurso", curso);

        }
    }
}
