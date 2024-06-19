using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class DocenteHorario
{
    public int IdDocenteHorario { get; set; }

    public int IdDocente { get; set; }

    public int IdHorario { get; set; }

    public virtual Docente IdDocenteNavigation { get; set; } = null!;

    public virtual Horario IdHorarioNavigation { get; set; } = null!;
}
