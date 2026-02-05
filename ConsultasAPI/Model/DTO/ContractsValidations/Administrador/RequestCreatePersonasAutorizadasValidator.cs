using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestCreatePersonasAutorizadasValidator : AbstractValidator<RequestCreatePersonasAutorizadas>
    {
        public RequestCreatePersonasAutorizadasValidator()
        {
           RuleFor(c => c.rfc)
                  .NotNull()
                  .NotEmpty()
                  .WithMessage("rfc es requerido");
                  
            RuleFor(c => c.nombre).NotNull()
                .NotNull()
                .NotEmpty()
                .WithMessage("nombre es requerido");

           
        }
    }
}
