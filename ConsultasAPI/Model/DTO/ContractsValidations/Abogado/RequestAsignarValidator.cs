using ConsultasAPI.Model.DTO.Contracts.Abogado;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Abogado
{
    public class RequestAsignarValidator : AbstractValidator<RequestAsignarAbogado>
    {

        public RequestAsignarValidator()
        {
            RuleFor(c => c.id_Consulta)
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