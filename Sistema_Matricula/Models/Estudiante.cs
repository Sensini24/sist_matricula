using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Estudiante
{
    public int IdEstudiante { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateOnly FechNacimiento { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string Direccion { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public virtual ICollection<ApoderadoAlumno> ApoderadoAlumnos { get; set; } = new List<ApoderadoAlumno>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
