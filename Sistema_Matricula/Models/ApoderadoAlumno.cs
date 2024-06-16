using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class ApoderadoAlumno
{
    public int IdApoderadoAlumno { get; set; }

    public string Parentesco { get; set; } = null!;

    public int? IdEstudiante { get; set; }

    public int? IdApoderado { get; set; }

    public virtual Apoderado? IdApoderadoNavigation { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }
}
