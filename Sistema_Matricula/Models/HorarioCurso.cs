using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class HorarioCurso
{
    public int IdHorarioCurso { get; set; }

    public int IdCurso { get; set; }

    public int IdHorario { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Horario IdHorarioNavigation { get; set; } = null!;
}
