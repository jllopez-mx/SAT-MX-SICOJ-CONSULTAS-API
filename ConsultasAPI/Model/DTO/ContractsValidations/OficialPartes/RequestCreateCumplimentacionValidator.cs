using FluentValidation;

namespace ConsultasAPI.Model.DTO.ContractsValidations
{
    public class RequestCreateCumplimentacionValidator : AbstractValidator<RequestCreateCumplimentacion>
    {
        public RequestCreateCumplimentacionValidator()
        {
            


            
        }
        private bool BeValidateDateFormar(string dateString)
        {
            return DateTime.TryParse(dateString, out _);
        }
    }
}
