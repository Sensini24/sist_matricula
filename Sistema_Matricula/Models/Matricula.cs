using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Matricula
{
    public int IdMatricula { get; set; }

    public DateTime FechMatricula { get; set; }

    public int? IdNivel { get; set; }

    public int? IdGrado { get; set; }

    public int? IdSeccion { get; set; }

    public int? IdPeriodEscolar { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdMonto { get; set; }

    public string? Estado { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Grado? IdGradoNavigation { get; set; }

    public virtual Monto? IdMontoNavigation { get; set; }

    public virtual Nivel? IdNivelNavigation { get; set; }

    public virtual PeriodoEscolar? IdPeriodEscolarNavigation { get; set; }

    public virtual Seccion? IdSeccionNavigation { get; set; }
}
