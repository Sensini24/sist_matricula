using Sistema_Matricula.Models;

namespace Sistema_Matricula.ViewsModels
{
    public class EstudianteCursoSeccionViewModel
    {
        public Matricula Matricula { get; set; }
        public Seccion Seccion { get; set; }
        public Grado Grado { get; set; }
        public Nivel Nivel { get; set; }
        public CursoSeccion CursoSeccion { get; set; }
        public Docente Docente { get; set; }
        public Estudiante Estudiante { get; set; }
        public Curso Curso { get; set; }
    }
}
