using FluentValidation;


namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestDeleteArchivosConsultaValidator :AbstractValidator<RequestDeleteArchivosConsulta>
    {
          public RequestDeleteArchivosConsultaValidator()
        {
           RuleFor(c => c.id)  
                .NotEmpty()
                .WithMessage("Alguno de los datos no tiene el formato correcto");
        }
    }
}