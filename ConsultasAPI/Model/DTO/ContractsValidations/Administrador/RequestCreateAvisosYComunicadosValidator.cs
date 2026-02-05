using ConsultasAPI.Model.DTO.Contracts.Administrador;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Administrador
{
    public class RequestCreateAvisosYComunicadosValidator : AbstractValidator<RequestCreateAvisosYComunicados>
    {
        public RequestCreateAvisosYComunicadosValidator()
        {
            RuleFor(c => c.idConsulta)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

            RuleFor(c => c.idTipoAviso)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("El valor indicado para el tipo de aviso debe ser un valor mayor a cero.");

            RuleFor(c => c.fechaIngreso)
            .NotNull()
            .NotEmpty()
            .WithMessage("Fecha de ingreso es requerida.")
            .Must(BeValidateDateFormar)
            .WithMessage("El formato de la fecha de ingreso no es válido");

        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}