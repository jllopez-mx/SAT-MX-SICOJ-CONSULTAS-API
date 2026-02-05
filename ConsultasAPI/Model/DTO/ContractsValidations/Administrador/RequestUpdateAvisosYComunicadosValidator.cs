using ConsultasAPI.Model.DTO.Contracts.Administrador;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Administrador
{
    public class RequestUpdateAvisosYComunicadosValidator  : AbstractValidator<RequestUpdateAvisosYComunicados>
    {
        
        public RequestUpdateAvisosYComunicadosValidator()
        {
            
             RuleFor(c => c.idConsulta)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

             RuleFor(c => c.id)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");


        }
    }
}