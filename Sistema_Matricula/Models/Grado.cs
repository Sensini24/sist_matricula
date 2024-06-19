using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Grado
{
    public int IdGrado { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
