using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Estudiante
{
    public int IdEstudiante { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Sexo { get; set; }

    public DateTime FechNacimiento { get; set; }

    public string Direccion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public int? IdUsuario { get; set; }

    public virtual ICollection<ApoderadoAlumno> ApoderadoAlumnos { get; set; } = new List<ApoderadoAlumno>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
