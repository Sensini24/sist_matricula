using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewComponents
{
    public class EditarCursitoVC: ViewComponent
    {
        private readonly DbMatNotaHorarioContext db;

        public EditarCursitoVC(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idcurso)
        {
            var curso = await db.Cursos.FindAsync(idcurso);
            //var sexos = new List<SelectListItem>
            //{
            //    new SelectListItem { Value = "Masculino", Text = "Masculino" },
            //    new SelectListItem { Value = "Femenino", Text = "Femenino" }
            //};

            //ViewBag.Sexos = sexos;

            if (curso == null)
            {
                // Manejar el caso en que el estudiante no se encuentra
                return Content("Curso no encontrado");
            }
            return View("_EditarCurso", curso);
        }

    }
}
