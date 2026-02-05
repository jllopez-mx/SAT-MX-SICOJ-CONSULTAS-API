using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestUpdateResolucionCumplimentacionValidator : AbstractValidator<RequestUpdateResolucionCumplimentacion>
    {
        public RequestUpdateResolucionCumplimentacionValidator()
        {
            RuleFor(c => c.idCumplimentacion)
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
