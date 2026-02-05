using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestUpdateCumplimentacionValidatro :AbstractValidator<RequestUpdateCumplimentacion>
    {
        public RequestUpdateCumplimentacionValidatro ()
        {
            

            RuleFor(c => c.fechaRecepcion)
                .NotEmpty()
                .Must(BeValidateDateFormar)
                .WithMessage("fecha_recepcion es requerida");
            
            RuleFor(c => c.idAdministracion).NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.plazoCumplimentar)
                .NotNull()
                .NotEmpty()
                .WithMessage("Plazo cumplimentar es requerid0.")
                .Must(value => value == 3 || value == 1 || value == 2)
                .WithMessage("El valor de Plazo cumplimentar debe ser 0, 1 o 2.");

        }
            private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
        
    }
}