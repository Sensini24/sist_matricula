using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Seccion
{
    public int IdSeccion { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdGrado { get; set; }

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();

    public virtual ICollection<CursoSeccion> CursoSeccions { get; set; } = new List<CursoSeccion>();

    public virtual Grado? IdGradoNavigation { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
}
