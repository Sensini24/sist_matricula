namespace Sistema_Matricula.ViewsModels
{
    public class MatriculaViewModel
    {
        public int IdMatricula { get; set; }

        public DateTime FechMatricula { get; set; }

        public int? IdNivel { get; set; } 
        public string DescripcionNivel { get; set; }

        public int? IdGrado { get; set; }
        public string DescripcionGrado { get; set; }

        public int? IdSeccion { get; set; }
        public string NombreSeccion { get; set; }
        public int? IdPeriodEscolar { get; set; }
        public string NombrePeriodo { get; set; }

        public int? IdEstudiante { get; set; }
        public string NombreEstudiante { get; set; }
        public string ApellidoEstudiante { get; set; }
        public int? IdMonto { get; set; }
        public string DescripcionMonto { get; set; }
        public decimal Monto { get; set; }

        public string? Estado { get; set; }
    }
}
