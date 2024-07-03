using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sistema_Matricula.ViewsModels
{
    public class RegistroEstudianteViewModel
    {
        public int IdEstudiante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechNacimiento { get; set; }
        public string? Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; } = null!;

        [BindNever]
        public string Estado { get; set; } = null!;
        public string Dni { get; set; } = null!;

        public int IdApoderado { get; set; }
        public string NombreApoderado { get; set; } = null!;
        public string ApellidoApoderado { get; set; } = null!;
        public int Edad { get; set; }
        public string Sexo { get; set; } = null!;
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
