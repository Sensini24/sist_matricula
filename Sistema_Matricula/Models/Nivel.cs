using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Grado> Grados { get; set; } = new List<Grado>();

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
