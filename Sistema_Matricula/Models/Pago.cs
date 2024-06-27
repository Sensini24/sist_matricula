using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public decimal MontoPago { get; set; }

    public DateTime FechPago { get; set; }

    public string TipoPago { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public int IdMatricula { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Matricula IdMatriculaNavigation { get; set; } = null!;
}
