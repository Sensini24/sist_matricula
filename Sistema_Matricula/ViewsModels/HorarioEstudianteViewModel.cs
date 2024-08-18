namespace Sistema_Matricula.ViewsModels
{
    public class HorarioEstudianteViewModel
    {
        public int IdCurso { get; set; }
        public string CursoNombre { get; set; }
        public int IdHorario { get; set; }
        public string Dia { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int? IdDocente { get; set; }
        public string NombreCompletoDocente { get; set; }

    }
}
