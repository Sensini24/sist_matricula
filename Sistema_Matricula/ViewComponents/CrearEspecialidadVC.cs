using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewComponents
{
    public class CrearEspecialidadVC : ViewComponent
    {
        private readonly DbMatNotaHorarioContext db;
        public CrearEspecialidadVC(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var especialidad = new Especialidad();
            return View("_CrearEspecialidadVC", especialidad);
        }
    }
}
