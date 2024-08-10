using FluentValidation;
using Sistema_Matricula.Models;

namespace Sistema_Matricula.Validaciones
{
    public class ValidacionAula : AbstractValidator<Aula>
    {
        public ValidacionAula()
        {
            RuleFor(e => e.Capacidad).NotEmpty().LessThan(25).WithMessage("La capacidad de cada aula debe ser menor a 25 estudiantes");
            RuleFor(e => e.VacantLibres).NotEmpty().GreaterThan (0).WithMessage("Las vacantes deben ser mayor a uno");
            RuleFor(e => e.IdSeccion).NotEmpty().WithMessage("Elige una sección");}
    }
}
