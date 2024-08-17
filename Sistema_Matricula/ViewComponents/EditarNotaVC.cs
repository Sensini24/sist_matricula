using Microsoft.AspNetCore.Mvc;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewComponents
{
    public class EditarNotaVC : ViewComponent
    {
        private readonly DbMatNotaHorarioContext db;

        public EditarNotaVC(DbMatNotaHorarioContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int idNota)
        {
            var nota = await db.Nota.FindAsync(idNota);
            return View("_EditarNotaModal", nota);
        }

    }
}
