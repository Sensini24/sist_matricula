using FluentValidation;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Validaciones
{
    public class ValidacionEstudiante : AbstractValidator<Estudiante>
    {
        public ValidacionEstudiante()
        {
            RuleFor(e => e.Nombre).NotEmpty().WithMessage("Se requiere el nombre");
            RuleFor(e => e.Apellido).NotEmpty().WithMessage("Se requiere el apellido");
            RuleFor(e => e.FechNacimiento).NotEmpty().WithMessage("Se requiere la fecha");
            RuleFor(e => e.Direccion).NotEmpty().WithMessage("Se requiere la dirección");
            RuleFor(e => e.Estado).NotEmpty().WithMessage("Se requiere el Estado");
            RuleFor(e => e.Dni).NotEmpty().WithMessage("Se requiere el DNI").MinimumLength(8).WithMessage("Son minimo 8 números");
        }
    }
}
