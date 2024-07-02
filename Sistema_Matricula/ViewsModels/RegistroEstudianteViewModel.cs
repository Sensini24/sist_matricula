namespace Sistema_Matricula.ViewsModels
{
    public class RegistroEstudianteViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechNacimiento { get; set; }
        public string? Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Dni { get; set; } = null!;

        public string NombreApoderado { get; set; } = null!;

        public string ApellidoApoderado { get; set; } = null!;

        public int Edad { get; set; }

        public string Sexo { get; set; } = null!;

        public string Ocupacion { get; set; } = null!;

        public string TelefonoApoderado { get; set; } = null!;

        public string DireccionApoderado { get; set; } = null!;


        public string Password { get; set; }
        public string ConfirmPassword { get; set; }


    }
}
