using System;
using System.Collections.Generic;

namespace Sistema_Matricula.Models;

public partial class UsuarioRol
{
    public int IdUsuarioRol { get; set; }

    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
