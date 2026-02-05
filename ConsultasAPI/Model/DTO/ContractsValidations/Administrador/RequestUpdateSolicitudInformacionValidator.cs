using ConsultasAPI.Model.DTO.Contracts.Administrador;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Administrador
{
    public class RequestUpdateSolicitudInformacionValidator: AbstractValidator<RequestUpdateSolicitudInformacion>
    {
          public RequestUpdateSolicitudInformacionValidator()
        {
            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");
             
            RuleFor(c => c.idUnidadAdministrativa)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.")   
                .When(c => c.unidadEsInterna);

            RuleFor(c => c.noOficioSolicitud)
                .NotEmpty()
                .WithMessage("No ha indicado el parámetro de número de oficio de solicitud.")
                .Matches(@"^[A-Za-z0-9/()-]*$")
                .WithMessage("El número de oficio contiene caracteres no permitidos. Solo se permiten letras, números, diagonal (/), guion medio (-) y paréntesis ().")
                .Must(x => !x.Contains(" "))
                .WithMessage("El número de oficio de solicitud no debe contener espacios en blanco.");

             RuleFor(c => c.noOficioRespuesta)
                .NotEmpty()
                .WithMessage("No ha indicado el parámetro de número de oficio de respuesta.")
                .Matches(@"^[A-Za-z0-9/()-]*$")
                .WithMessage("El número de oficio contiene caracteres no permitidos. Solo se permiten letras, números, diagonal (/), guion medio (-) y paréntesis ().")
                .Must(x => !x.Contains(" "))
                .WithMessage("El número de oficio de respuesta no debe contener espacios en blanco.");

            RuleFor(c => c.fechaOficioSolicitud)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fecha Requerimiento es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de oficio de solicitud no es válido");

           
        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}