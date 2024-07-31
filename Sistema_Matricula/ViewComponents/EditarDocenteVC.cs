using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewComponents
{
    public class EditarDocenteVC : ViewComponent
    {
        private readonly DbMatNotaHorarioContext db;

        public EditarDocenteVC(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int iddocente)
        {

            var docente = await db.Docentes.FindAsync(iddocente);

            var sexos = new List<SelectListItem>
            {
                new SelectListItem { Value = "Masculino", Text = "Masculino" },
                new SelectListItem { Value = "Femenino", Text = "Femenino" }
            };
            var estado = new List<SelectListItem>
            {
                new SelectListItem { Value = "Activo", Text = "Activo" },
                new SelectListItem { Value = "Inactivo", Text = "Inactivo" }
            };

            ViewBag.Especialidades = new SelectList(db.Especialidads, "IdEspecialidad", "Especialidad1").ToList();
            ViewBag.Sexos = sexos;
            ViewBag.Estados = estado;

            return View("_EditarDocente", docente);
        }
    }
}
