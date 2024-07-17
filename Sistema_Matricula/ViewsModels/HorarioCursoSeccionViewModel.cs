using Sistema_Matricula.Models;
namespace Sistema_Matricula.ViewsModels
{
    public class HorarioCursoSeccionViewModel
    {
        public int IdHorario { get; set; }
        public int IdCursoSeccion { get; set; }
        public int IdHorarioCursoSeccion { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int IdSeccion { get; set; }
        public string DiaSemana { get; set; }
    }
}
