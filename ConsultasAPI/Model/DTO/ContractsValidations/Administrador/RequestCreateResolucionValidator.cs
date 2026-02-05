using FluentValidation;
using System.Data;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestCreateResolucionValidator : AbstractValidator<RequestCreateResolucion>
    {
        public RequestCreateResolucionValidator()
        {
            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.noOficioResolucion)
                .NotEmpty()
                .WithMessage("No ha indicado el parámetro de número de oficio.")
                .Matches(@"^[A-Za-z0-9/()-]*$")
                .WithMessage("El número de oficio contiene caracteres no permitidos. Solo se permiten letras, números, diagonal (/), guion medio (-) y paréntesis ().")
                .Must(x => x.Length <= 40)
                .WithMessage("El número de oficio excede el máximo de 40 caracteres.")
                .Must(x => !x.Contains(" "))
                .WithMessage("El número de oficio no debe contener espacios en blanco.");

            RuleFor(c => c.fechaResolucion)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fecha Resolucion es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de Resolucion no es válido");

            RuleFor(c => c.idSentido)
                .NotNull()
                .WithMessage("Sentido es requerido.")
                .GreaterThan(0)
                .WithMessage("Sentido requiere un valor mayor a 0.");

            RuleFor(c => c.idSeccion)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el idseccion debe ser un valor mayor a cero.");

            RuleFor(c => c.idTipoArchivo)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el tipo de archivo debe ser un valor mayor a cero.");
            
            RuleFor(c => c.documento)
                .NotNull()
                .NotEmpty()
                .WithMessage("El valor indicado para el document no  debe ser un vacio");
           

        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}
