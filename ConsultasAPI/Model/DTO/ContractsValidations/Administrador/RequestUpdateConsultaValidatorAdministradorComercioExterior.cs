using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestUpdateConsultaValidatorAdministradorComercioExterior :AbstractValidator<RequestUpdateConsultaAdministradorComercioExterior>
    {
        public RequestUpdateConsultaValidatorAdministradorComercioExterior ()
        {
            

            RuleFor(c => c.promoventeEsContribuyente)
                .NotNull()
                .WithMessage("No ha indicado el parametro de promovente_es_contribuyente");

            RuleFor(c => c.rfcContribuyente)
                .Matches(@"^(?<pf>[A-Z]{4}\d{6}[A-Z0-9]{3})|(?<pm>[A-Z]{3}\d{6}[A-Z0-9]{3})$")
                .Unless(c => c.promoventeEsContribuyente)
                .WithMessage("El RFC no es válido");

                RuleFor(c => c.contribuyente)
                .NotEmpty()
                .NotNull()
                .Matches(@"^([A-Za-zÑñÁáÉéÍíÓóÚú]+['\-]{0,1}[A-Za-zÑñÁáÉéÍíÓóÚú]+)(\s+([A-Za-zÑñÁáÉéÍíÓóÚú]+['\-]{0,1}[A-Za-zÑñÁáÉéÍíÓóÚú]+))*$")
                .WithMessage("El parametro de nombre_contribuyente no tiene el formato correcto")
                .Unless(c => c.promoventeEsContribuyente);

            RuleFor(c => c.fechaPresentacion)
                .NotEmpty()
                .Must(BeValidateDateFormar)
                .WithMessage("fecha_presentacion es requerida");

            RuleFor(c => c.fechaRecepcion)
                .NotEmpty().WithMessage("fecha_recepcion es requerida")
                .GreaterThanOrEqualTo(c => c.fechaPresentacion)
                .WithMessage("la fecha de recepcion debe ser mayor o igual a la fecha de presentacion")
                .Must(BeValidateDateFormar);

            RuleFor(c => c.idTipoAsunto)
               .NotNull()
               .WithMessage("No ha indicado el parametro de tipo_asunto")
               .WithMessage("Identificador de tipo de asunto tiene que se un valor mayor a cero.");
            RuleFor(c => c.idTipoModalidad)
               .NotNull()
               .WithMessage("No ha indicado el parametro de tipo_entrada")
               .WithMessage("Identificador de tipo de entrada tiene que se un valor mayor a cero.");            
            

        }
            private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
        
    }
}