using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Curso
{
    public int IdCurso { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<HorarioCurso> HorarioCursos { get; set; } = new List<HorarioCurso>();

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
