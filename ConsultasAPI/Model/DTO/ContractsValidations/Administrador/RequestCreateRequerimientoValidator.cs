using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestCreateRequerimientoValidator : AbstractValidator<RequestCreateRequerimiento>
    {
        public RequestCreateRequerimientoValidator()
        {
            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.noOficioRequerimiento)
                .NotEmpty()
                .WithMessage("No ha indicado el parámetro de número de oficio.")
                .Matches(@"^[A-Za-z0-9/()-]*$")
                .WithMessage("El número de oficio contiene caracteres no permitidos. Solo se permiten letras, números, diagonal (/), guion medio (-) y paréntesis ().")
                .Must(x => x.Length <= 40)
                .WithMessage("El número de oficio excede el máximo de 40 caracteres.")
                .Must(x => !x.Contains(" "))
                .WithMessage("El número de oficio no debe contener espacios en blanco.");

            RuleFor(c => c.fechaRequerimiento)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fecha Requerimiento es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de Requerimiento no es válido");

                      

        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}
