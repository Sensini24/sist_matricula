using Sistema_Matricula.Models;
namespace Sistema_Matricula.ViewsModels
{
    public class HorarioCursoViewModel
    {
        public int IdCurso { get; set; }
        public int IdHorario { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int IdAula { get; set; }
        public int IdSeccion { get; set; }
        public string DiaSemana { get; set; }
    }
}
