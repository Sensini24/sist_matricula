using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public int? IdDocente { get; set; }

    public int? IdAula { get; set; }

    public int? IdSeccion { get; set; }

    public virtual ICollection<HorarioCurso> HorarioCursos { get; set; } = new List<HorarioCurso>();

    public virtual Aula? IdAulaNavigation { get; set; }

    public virtual Docente? IdDocenteNavigation { get; set; }

    public virtual Seccion? IdSeccionNavigation { get; set; }
}
