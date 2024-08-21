using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class PeriodoEscolar
{
    public int IdPeriodEscolar { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime FechInicio { get; set; }

    public DateTime FechFinal { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
