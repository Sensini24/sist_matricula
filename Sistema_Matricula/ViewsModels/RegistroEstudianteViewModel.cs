using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sistema_Matricula.ViewsModels
{
    public class RegistroEstudianteViewModel
    {
        public int IdEstudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string SexoEstudiante { get; set; }
        public DateTime FechNacimiento { get; set; }
        public string Direccion { get; set; } = null!;
        public int IdUsuarioEstudiante { get; set; }

        [BindNever]
        public string Estado { get; set; } = null!;
        public string Dni { get; set; } = null!;

        public int IdApoderado { get; set; }
        public string NombreApoderado { get; set; } = null!;
        public string ApellidoApoderado { get; set; } = null!;
        public DateTime FechNacimientoApoderado { get; set; }
        public string Sexo { get; set; }
        public string Ocupacion { get; set; } = null!;
        public string TelefonoApoderado { get; set; } = null!;
        public string DireccionApoderado { get; set; } = null!;

        public int IdApoderadoAlumno { get; set; }
        public string Parentesco { get; set; } = null!;
        public int? IdEstudianteIntermedio { get; set; }
        public int? IdApoderadoIntermedio { get; set; }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string PasswordHash { get; set; }

    }
}
