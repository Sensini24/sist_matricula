using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Factura
{
    public int IdFactura { get; set; }

    public int IdEstudiante { get; set; }

    public int IdApoderado { get; set; }

    public int IdPago { get; set; }

    public DateOnly FechEmision { get; set; }

    public decimal MontoTotal { get; set; }

    public int IdConcepto { get; set; }

    public virtual Apoderado IdApoderadoNavigation { get; set; } = null!;

    public virtual Concepto IdConceptoNavigation { get; set; } = null!;

    public virtual Estudiante IdEstudianteNavigation { get; set; } = null!;

    public virtual Pago IdPagoNavigation { get; set; } = null!;
}
