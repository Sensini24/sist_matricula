using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Notum
{
    public int IdNota { get; set; }

    public decimal Nota { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? IdCurso { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdBimestre { get; set; }

    public virtual Bimestre? IdBimestreNavigation { get; set; }

    public virtual Curso? IdCursoNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }
}
