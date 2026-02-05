using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestUpdatePersonasAutorizadasValidator :AbstractValidator<RequestUpdatePersonasAutorizadas>
    {
        public RequestUpdatePersonasAutorizadasValidator ()
        {
            RuleFor(c => c.rfc)
                  .NotNull()
                  .NotEmpty()
                  .WithMessage("rfc es requerido");

            RuleFor(c => c.nombre)
                .NotEmpty()
                .NotNull()
                .Matches(@"^([A-Za-zÑñÁáÉéÍíÓóÚú]+['\-]{0,1}[A-Za-zÑñÁáÉéÍíÓóÚú]+)(\s+([A-Za-zÑñÁáÉéÍíÓóÚú]+['\-]{0,1}[A-Za-zÑñÁáÉéÍíÓóÚú]+))*$")
                .WithMessage("El parametro de nombre no tiene el formato correcto");

           
        }
    }
}