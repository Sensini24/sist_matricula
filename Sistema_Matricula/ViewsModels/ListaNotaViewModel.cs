namespace Sistema_Matricula.ViewsModels
{
    public class ListaNotaViewModel
    {
        public int IdEstudiante { get; set; }
        public string NombreCompleto { get; set; }
        public string Descripcion { get; set; }
        public decimal? Nota { get; set; }
        public int IdNota { get; set; }
        public string Bimestre  { get; set; }
        public int? IdBimestre { get; set; }

    }
}
