using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestBuscaConsultaRfcValidator:AbstractValidator<RequestBuscaConsultaRfc>
    {
        public RequestBuscaConsultaRfcValidator ()
        {
            RuleFor(c => c.nombre)
                .NotNull()
                .NotEmpty()
                .WithMessage("RFC es obligatorio.");
        }
    }
}