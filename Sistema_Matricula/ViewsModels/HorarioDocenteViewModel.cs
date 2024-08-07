namespace Sistema_Matricula.ViewsModels
{
    public class HorarioDocenteViewModel
    {
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string DiaSemana { get; set; }
        public string Curso { get; set; }
        public string Seccion { get; set; }
        public string Grado { get; set; }
        public string Nivel { get; set; }
        public int IdDocente { get; set; }
        public int IdCurso { get; set; }
        public int IdSeccion { get; set; }
        public int IdGrado { get; set; }
        public int IdNivel { get; set; }
    }
}
