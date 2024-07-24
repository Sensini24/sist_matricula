using FluentValidation;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Validaciones
{
    public class ValidacionApoderado : AbstractValidator<Apoderado>
    {
        public ValidacionApoderado()
        {
            RuleFor(e => e.Nombre);
            RuleFor(e => e.Apellido);
            RuleFor(e => e.FechNacimiento).NotEmpty().WithMessage("Se requiere la fecha");
            RuleFor(e => e.Sexo).NotEmpty().WithMessage("Se requiere el Email").EmailAddress().WithMessage("El email no es válido");
            RuleFor(e => e.Ocupacion).NotEmpty().WithMessage("Se requiere el telefono").MinimumLength(9).WithMessage("Son mínimo 9 números");
            RuleFor(e => e.Telefono).NotEmpty().WithMessage("Se requiere la dirección");
            RuleFor(e => e.Direccion).NotEmpty().WithMessage("Se requiere el Estado");
        }
    }
}
