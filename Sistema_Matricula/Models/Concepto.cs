﻿using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Concepto
{
    public int IdConcepto { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
