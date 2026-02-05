using ConsultasAPI.Model.DTO.Contracts.Administrador;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Administrador
{
    public class RequestAsignarCumplimentacionValidator : AbstractValidator<RequestAsignarCumplimentacionAdministrador>
    {

        public RequestAsignarCumplimentacionValidator()
        {
            RuleFor(c => c.idCumplimentacion)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.idAbogado).NotNull()
                .NotNull()
                .NotEmpty()
                .WithMessage("nombre es requerido");

            RuleFor(c => c.idAdministracion)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.idSubadministracion)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");


        }
    }
}