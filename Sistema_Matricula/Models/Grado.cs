using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Grado
{
    public int IdGrado { get; set; }

    public string Descripcion { get; set; } = null!;

    public int? IdNivel { get; set; }

    public virtual Nivel? IdNivelNavigation { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public virtual ICollection<Seccion> Seccions { get; set; } = new List<Seccion>();
}
