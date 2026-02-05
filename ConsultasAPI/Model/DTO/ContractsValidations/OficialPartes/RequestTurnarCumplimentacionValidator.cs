using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestTurnarCumplimentacionValidator: AbstractValidator<RequestTurnarCumplimentacion>
    {
        
        public RequestTurnarCumplimentacionValidator()
        {
            RuleFor(c => c.id)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");  

            RuleFor(c => c.idAdministracion)
               .NotEmpty()
               .GreaterThan(0)
               .WithMessage("El valor indicado para la administracion debe ser un valor mayor a cero.");         
           
        }
    }
}