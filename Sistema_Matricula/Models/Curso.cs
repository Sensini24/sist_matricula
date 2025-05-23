﻿using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Curso
{
    public int IdCurso { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<CursoDocente> CursoDocentes { get; set; } = new List<CursoDocente>();

    public virtual ICollection<CursoSeccion> CursoSeccions { get; set; } = new List<CursoSeccion>();

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
