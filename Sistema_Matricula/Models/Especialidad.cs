using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Especialidad
{
    public int IdEspecialidad { get; set; }

    public string Especialidad1 { get; set; } = null!;

    public virtual ICollection<Docente> Docentes { get; set; } = new List<Docente>();
}
