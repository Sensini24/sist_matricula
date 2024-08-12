namespace Sistema_Matricula.ViewsModels
{
    public class CursosYDetallesdeDocente
    {
        public int IdCursoDocente { get; set; }
        public int IdDocente { get; set; }
        public string NombreDocente { get; set; }
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public int cantidadAlumnos { get; set; }
    }
}
