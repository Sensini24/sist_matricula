using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var sexos = new List<SelectListItem>
            {
                new SelectListItem { Value = "Masculino", Text = "Masculino" },
                new SelectListItem { Value = "Femenino", Text = "Femenino" }
            };

            ViewBag.Sexos = sexos;

            if (estudiante == null)
            {
                // Manejar el caso en que el estudiante no se encuentra
                return Content("Estudiante no encontrado");
            }
            return View("_EditarEstudiante", estudiante);
        }
    }
}
