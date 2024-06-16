using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Monto
{
    public int IdMonto { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Monto1 { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
