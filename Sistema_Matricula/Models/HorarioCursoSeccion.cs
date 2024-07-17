using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class HorarioCursoSeccion
{
    public int IdHorarioCursoSeccion { get; set; }

    public int? IdCursoSeccion { get; set; }

    public int IdHorario { get; set; }

    public virtual CursoSeccion? IdCursoSeccionNavigation { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; } = null!;
}
