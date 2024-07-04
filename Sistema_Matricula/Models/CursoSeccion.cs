using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class CursoSeccion
{
    public int IdCursoSeccion { get; set; }

    public int IdCurso { get; set; }

    public int IdSeccion { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Seccion IdSeccionNavigation { get; set; } = null!;
}
