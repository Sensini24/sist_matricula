using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Aula
{
    public int IdAula { get; set; }

    public int Capacidad { get; set; }

    public int VacantLibres { get; set; }

    public int? IdSeccion { get; set; }

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();

    public virtual Seccion? IdSeccionNavigation { get; set; }
}
