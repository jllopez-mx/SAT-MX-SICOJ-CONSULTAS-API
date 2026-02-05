using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations.Administrador
{
    public class RequestSolicitaRequerimientoValidator: AbstractValidator<RequestSolicitaRequerimiento>
    {
          public RequestSolicitaRequerimientoValidator()
        {
            RuleFor(c => c.idConsulta)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

    
        }
    }
}