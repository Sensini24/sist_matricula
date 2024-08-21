namespace Sistema_Matricula.ViewsModels
{
    public class PagoEstudianteViewModel
    {
        public int IdMatricula { get; set; }
        public string NombreEstudiante { get; set; }
        public string ApellidoEstudiante { get; set; }
        public decimal Monto { get; set; }
        public string TipoPago { get; set; }
        public string Concepto { get; set; }
        public DateTime FechaPago { get; set; }
        public string Estado { get; set; }

    }
}
