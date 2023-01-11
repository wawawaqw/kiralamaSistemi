using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.DataAccess.Sevices
{
    public class OzelException : Exception
    {
        public List<Error>? Errors;


        public OzelException() : base()
        {

        }

        public OzelException(Error error)
        {
            this.Errors = new List<Error>() { error };
        }

        public OzelException(string message, Exception? ex = null)
         : base(message, ex)

        {
            this.Errors = new List<Error>() { new Error("danger", message) };
        }

        public OzelException(string message, List<Error> errors)
          : base(message)
        {
            this.Errors = errors;
        }

        public OzelException(params Error[] errors)
         : base(errors.FirstOrDefault(i => !String.IsNullOrWhiteSpace(i.Title))?.Title)
        {
            this.Errors = errors.ToList();
        }

        public OzelException(List<Error> errors)
         : base(errors.FirstOrDefault(i => !String.IsNullOrWhiteSpace(i.Title))?.Title)
        {
            this.Errors = errors;
        }


        public OzelException(string message, params Error[] errors)
            : base(message)
        {
            this.Errors = errors.Select(i => new Error(i.Code, i.Description) { Key = i.Key ?? EnumErrorTypes.danger, Title = message ?? i.Title }).ToList();

        }

        public OzelException(String message, Exception innerException, List<Error> errors)
            : base(message, innerException)
        {
            this.Errors = errors;
        }

        public OzelException(String message, Exception innerException, params Error[] errors)
            : base(message, innerException)
        {
            this.Errors = errors.ToList();
        }


    }
}
