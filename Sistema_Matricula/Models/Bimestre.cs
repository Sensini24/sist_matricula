using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Bimestre
{
    public int IdBimestre { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
