using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestCreaArchivoConsultaValidator :AbstractValidator<RequestCreaArchivoConsulta>
    {
        public RequestCreaArchivoConsultaValidator()
        {

            RuleFor(c => c.idConsulta)
                .GreaterThan(0)
                .WithMessage("id_consulta del registro tiene que se un valor mayor a cero.");

            RuleFor(c => c.idSeccion)
                .NotNull()
                .NotEmpty()
                .WithMessage("Debe indicar un valor para id_seccion");

            RuleFor(c => c.fileConsultas)
                .NotNull()
                .NotEmpty()
                .WithMessage("Debe subir un archivo")
                .Must(file => file.Length <= 10737418240)
                .WithMessage("El archivo no debe pesar más de 10 GB")
                .Must(file => file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                .WithMessage("El archivo debe ser un .pdf");
        }
    }
}