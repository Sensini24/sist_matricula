namespace Sistema_Matricula.ViewsModels
{
    public class DeudaEstudianteViewModel
    {
        public int IdMatricula { get; set; }
        public string NombreEstudiante { get; set; }
        public string ApellidoEstudiante { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaEmision { get; set; }
    }
}
