using FluentValidation;
namespace ConsultasAPI.Model.DTO.ContractsValidations.Abogado
{
    public class RequestUpdateConsultaValidatorAbogadoImpuestosInternos: AbstractValidator<RequestUpdateConsultaAbogadoImpuestosInternos>
    {
       public RequestUpdateConsultaValidatorAbogadoImpuestosInternos()
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

            RuleFor(c => c.idTipoAsunto)
               .NotNull()
               .WithMessage("No ha indicado el parametro de tipo_asunto")
               .WithMessage("Identificador de tipo de asunto tiene que se un valor mayor a cero.");
            RuleFor(c => c.idTipoModalidad)
               .NotNull()
               .WithMessage("No ha indicado el parametro de tipo_entrada")
               .WithMessage("Identificador de tipo de entrada tiene que se un valor mayor a cero.");

            RuleFor(c => c.monto)
                .NotNull()
                .WithMessage("No ha indicado el parametro de tipo_asunto")
                .WithMessage("Identificador de tipo monto tiene que se un valor mayor a cero.");

            RuleFor(c => c.idTema)
                .NotNull()
                .WithMessage("No ha indicado el parametro de tema");

        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        } 
    }

}