using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewComponents
{
    public class EditarCursoVC : ViewComponent
    {
        private readonly DbMatNotaHorarioContext db;

        public EditarCursoVC(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idEstudiante)
        {
            var estudiante = await db.Estudiantes.FindAsync(idEstudiante);
            if (estudiante == null)
            {
                // Manejar el caso en que el estudiante no se encuentra
                return Content("Estudiante no encontrado");
            }
            return View("_EditarEstudiante", estudiante);
        }
    }
}
