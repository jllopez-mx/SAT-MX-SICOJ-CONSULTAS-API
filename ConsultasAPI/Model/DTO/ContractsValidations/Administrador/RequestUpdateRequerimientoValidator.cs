using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestUpdateRequerimientoValidator : AbstractValidator<RequestUpdateRequerimiento>
    {
        public RequestUpdateRequerimientoValidator()
        {
            RuleFor(c => c.id)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");         

            RuleFor(c => c.fechaNotificacion)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fecha Notificación es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de Notificación no es válido");
         
        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}
