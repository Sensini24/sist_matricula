using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class CursoDocente
{
    public int IdCursoDocente { get; set; }

    public int IdDocente { get; set; }

    public int IdCurso { get; set; }

    public virtual Curso IdCursoNavigation { get; set; } = null!;

    public virtual Docente IdDocenteNavigation { get; set; } = null!;
}
