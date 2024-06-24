using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class VistaCursoAlumno
{
    public int IdEstudiante { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? Nota { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Bimestre { get; set; } = null!;

    public string Profesor { get; set; } = null!;
}
