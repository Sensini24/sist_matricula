using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Apoderado
{
    public int IdApoderado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime? FechNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string Ocupacion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public virtual ICollection<ApoderadoAlumno> ApoderadoAlumnos { get; set; } = new List<ApoderadoAlumno>();
}
