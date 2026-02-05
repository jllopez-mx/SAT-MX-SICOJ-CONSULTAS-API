using FluentValidation;


namespace ConsultasAPI.Model.DTO.ContractsValidations
{
  public class RequestRemisionValidator : AbstractValidator<RequestRemision>
  {
    public RequestRemisionValidator()
    {
      RuleFor(c => c.idConsulta)
           .NotEmpty()
           .GreaterThan(0)
           .WithMessage("El valor indicado para el id debe ser un valor mayor a cero.");

      
      RuleFor(c => c.idAdministracionRemite)
        .NotEmpty()
        .GreaterThan(0)
        .WithMessage("El valor indicado para la unidad administrativa que remite debe ser un valor mayor a cero.");
      
      RuleFor(c => c.idTipoAutoridad)
        .NotEmpty()
        .GreaterThan(0)
        .WithMessage("El valor indicado para el tipo de autoridad debe ser un valor mayor a cero.");
      
      RuleFor(c => c.noOficioRemison)
          .NotEmpty()
          .WithMessage("No ha indicado el parametro de nomero de oficio");
          
      RuleFor(c => c.fechaOficio)
                .NotNull()
                .NotEmpty()
                .WithMessage("Fecha de oficio es requerida.")
                .Must(BeValidateDateFormar)
                .WithMessage("El formato de la fecha de oficio no es válido");
    }
     private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
  }
}