using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Grado
{
    public int IdGrado { get; set; }

    public string Descripcio { get; set; } = null!;

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
