using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestCreateConsultaValidator : AbstractValidator<RequestCreateConsulta>
    {
        public RequestCreateConsultaValidator()
        {
            RuleFor(c => c.rfc)
                  .NotNull()
                  .NotEmpty()
                  .WithMessage("rfc es requerido");
                  
            RuleFor(c => c.promovente)
                .NotNull()
                .NotEmpty()
                .WithMessage("promovente es requerido");

            RuleFor(c => c.rfcContribuyente)
                .NotEmpty()
                .WithMessage("El RFC del contribuyente es requerido.");                

            RuleFor(c => c.contribuyente)
                .NotEmpty()
                .WithMessage("El contribuyente  es requerido.");

            RuleFor(c => c.fechaPresentacion)
                .NotNull()
                .WithMessage("Fecha presentación es requerida.")
                .NotEmpty()
                .WithMessage("Fecha presentación es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de presentación no es válido");

            RuleFor(c => c.fechaRecepcion)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fecha recepción es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de recepción no es válido");

            RuleFor(c => c.idTipoAsunto)
                .NotNull()
                .WithMessage("Tipo asunto es requerido.")
                .GreaterThan(0)
                .WithMessage("Tipo asunto requiere un valor mayor a 0.");

            RuleFor(c => c.idTipoModalidad)
                .NotNull()
                .WithMessage("Tipo modalidad es requerido.")
                .GreaterThan(0)
                .WithMessage("Tipo modalidad requiere un valor mayor a 0.");

        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}
