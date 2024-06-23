using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Docente
{
    public int IdDocente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int Edad { get; set; }

    public string Sexo { get; set; } = null!;

    public string? Telefono { get; set; }

    public DateOnly FechNacimiento { get; set; }

    public int? IdEspecialidad { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<CursoDocente> CursoDocentes { get; set; } = new List<CursoDocente>();

    public virtual ICollection<DocenteHorario> DocenteHorarios { get; set; } = new List<DocenteHorario>();

    public virtual Especialidad? IdEspecialidadNavigation { get; set; }

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
