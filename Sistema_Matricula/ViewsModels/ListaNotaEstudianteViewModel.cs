namespace Sistema_Matricula.ViewsModels
{
    public class ListaNotaEstudianteViewModel
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; }
        public int IdBimestre { get; set; }
        public string NombreBimestre { get; set; }
        public int IdNota { get; set; }
        public decimal? Nota { get; set; }
        public string DescripcionNota { get; set; }
        public int IdEstudiante { get; set; }

    }
}
