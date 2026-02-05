using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestTurnarConsultaValidator: AbstractValidator<RequestTurnarConsulta>
    {
        
        public RequestTurnarConsultaValidator()
        {
            RuleFor(c => c.id)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");           
           
        }
    }
}