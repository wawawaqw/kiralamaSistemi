using kiralamaSistemi.DataAccess.Sevices;
using FluentValidation.Results;

namespace kiralamaSistemi.DataAccess.Extensions
{
    public static class FluentValidationExtensions
    {
        public static List<Error> GetListError(this ValidationResult validationResult, string Title = null)
        {

            return validationResult.Errors.Select(i => new Error(i.ErrorCode, Title, i.ErrorMessage)).ToList();
        }
    }
}


