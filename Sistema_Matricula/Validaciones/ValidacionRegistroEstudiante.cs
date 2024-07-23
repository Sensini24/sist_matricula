using Sistema_Matricula.ViewsModels;
using FluentValidation;

namespace Sistema_Matricula.Validaciones
{
    public class ValidacionRegistroEstudiante : AbstractValidator<RegistroEstudianteViewModel>
    {
        public ValidacionRegistroEstudiante(){
            RuleFor(e => e.Nombre).NotEmpty().WithMessage("Se requiere el nombre");
            RuleFor(e => e.Apellido).NotEmpty().WithMessage("Se requiere los apellidos");
            RuleFor(e => e.FechNacimiento).NotEmpty().WithMessage("Se requiere la fecha de nacimiento");
            RuleFor(e => e.Email).NotEmpty().WithMessage("Se requiere el email")
                .EmailAddress().WithMessage("Debe tener el formato email con '@'");
            RuleFor(e => e.Telefono).NotEmpty().WithMessage("Se requiere el numero de teléfono")
                .Length(11).WithMessage("Agregue un numero de telefono válido");
            RuleFor(e => e.Dni).NotEmpty().WithMessage("Se requiere el DNI");
            RuleFor(e => e.NombreApoderado).NotEmpty().WithMessage("Se requiere el nombre del Apoderado").Length(3).WithMessage("Ingrese un nombre válido");
            RuleFor(e => e.ApellidoApoderado).NotEmpty()
                .WithMessage("Se requiere el apellido del Apoderado").Length(3).WithMessage("Ingrese apellidos válidos");
            RuleFor(x => x.Edad)
                .InclusiveBetween(18,70).WithMessage("La edad debe estar entre 3 y 25 años.");
            RuleFor(e => e.Sexo).NotEmpty().WithMessage("Se requiere el sexo del Apoderado");
            RuleFor(e => e.Ocupacion).NotEmpty().WithMessage("Se requiere la ocupación del Apoderado");
            RuleFor(e => e.TelefonoApoderado).NotEmpty().WithMessage("Se requiere el apellido del Apoderado").MaximumLength(11).MinimumLength(8).WithMessage("Ingrese un numero de telefono válido");
            RuleFor(e => e.DireccionApoderado).NotEmpty().WithMessage("Se requiere la dirección del Apoderado");
            RuleFor(e => e.Parentesco).NotEmpty().WithMessage("Se requiere el parentesco que comparte del estudiante con el apoderado");
            RuleFor(e => e.Ocupacion).NotEmpty().WithMessage("Se requiere la ocupación del Apoderado");

        }
    }
}
