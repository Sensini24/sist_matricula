using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewsModels
{
    public class AsignacionNotasViewModel
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public int IdDocente { get; set; }
        public string NombreDocente { get; set; }
        public int IdSeccion { get; set; }
        public int IdBimestre { get; set; }
        public string Descripcion { get; set; }
        public List<NotaEstudianteViewModel> EstudiantesNotas { get; set; }

    }
}
