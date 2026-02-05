using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestUpdateResolucionValidator : AbstractValidator<RequestUpdateResolucion>
    {
        public RequestUpdateResolucionValidator()
        {
            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            
        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}
