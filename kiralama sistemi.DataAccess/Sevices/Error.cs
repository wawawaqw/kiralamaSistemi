using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.DataAccess.Sevices
{
    public class Error
    {
        public EnumErrorTypes? Key { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public Error() { }
        public Error(string? description)
        {
            Description = description;
        }
        public Error(string? code, string? description)
        {
            Description = description;
            Code = code;
        }
        public Error(string? code, string? title, string? description)
        {
            Title = title;
            Description = description;
            Code = code;
        }
        public Error(string? code, string? title, string? description, EnumErrorTypes? key)
        {
            Key = key;
            Description = description;
            Title = title;
            Code = code;
        }
        public Error DescriptionWithFormat(params string?[] data)
        {
            if (data?.Length > 0 && !string.IsNullOrWhiteSpace(Description))
            {
                Description = String.Format(Description, data);
            }
            return this;
        }
        public Error CustomCode(string? code)
        {
            if (code != null)
            {
                Code = code;
            }
            return this;
        }
    }
}
