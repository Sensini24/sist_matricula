using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Seccion
{
    public int IdSeccion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
