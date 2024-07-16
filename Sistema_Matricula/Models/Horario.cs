using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string? DiaSemana { get; set; }

    public virtual ICollection<HorarioCurso> HorarioCursos { get; set; } = new List<HorarioCurso>();
}
