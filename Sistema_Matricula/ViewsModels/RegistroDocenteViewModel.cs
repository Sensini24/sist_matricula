namespace Sistema_Matricula.ViewsModels
{
    public class RegistroDocenteViewModel
    {
        public int IdDocente { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public int Edad { get; set; }

        public string Sexo { get; set; } = null!;

        public string? Telefono { get; set; }

        public DateTime FechNacimiento { get; set; }

        public int? IdEspecialidad { get; set; }

        public string Estado { get; set; } = null!;

        public int? IdUsuarioDocente { get; set; }

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string PasswordHash { get; set; }

    }
}
