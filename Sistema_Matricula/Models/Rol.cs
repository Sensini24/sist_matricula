using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioRol> UsuarioRols { get; set; } = new List<UsuarioRol>();
}
