using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int IdPago { get; set; }

    public DateTime FechEmision { get; set; }

    public decimal MontoTotal { get; set; }

    public virtual Pago IdPagoNavigation { get; set; } = null!;
}
