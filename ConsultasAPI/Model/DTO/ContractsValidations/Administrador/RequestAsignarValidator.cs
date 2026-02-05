using ConsultasAPI.Model.DTO.Contracts.Administrador;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Administrador
{
    public class RequestAsignarValidator : AbstractValidator<RequestAsignarAdministrador>
    {

        public RequestAsignarValidator()
        {
            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.idAbogado).NotNull()
                .NotNull()
                .NotEmpty()
                .WithMessage("nombre es requerido");


        }
    }
}