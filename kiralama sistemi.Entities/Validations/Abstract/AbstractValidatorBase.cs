using FluentValidation;

namespace kiralamaSistemi.Entities.Validations.Abstract
{
    public abstract class AbstractValidatorBase<T> : AbstractValidator<T>
    {
        public AbstractValidatorBase()
        {

        }
        public readonly static CascadeMode cascadeMode = CascadeMode.StopOnFirstFailure;

        public readonly static int SiteLength = 9;
        public readonly static int SiteLengthPoint = 6;
        public static string GreaterThanOrEqualTo(string name)
        {
            return "Lütfen " + name + " 2000 den büyük bir tarih giriniz.";
        }
        public static string NotEmpty(string name)
        {
            return "Lütfen Geçerli " + name + " giriniz.";
        }
        public static string ScalePrecision(string name)
        {
            return name + " max 17 karakter olmalıdır.";
        }
        public static string Seciniz(string name)
        {
            return "Lütfen Geçerli Bir " + name + " Seçiniz.";
        }
        public static string Bulunmadi(string name)
        {
            return "Seçtiğiniz " + name + " bulunmadi.";
        }
        public static string NotNull(string name)
        {
            return "Lütfen " + name + " giriniz.";
        }
        public static string MaximumLength(string name)
        {
            return name + " max {MaxLength} karakter olmalıdır.";
        }
        public static string Length(string name)
        {
            return name + " Uzunluğu {Length} karakter olmalıdır.";
        }
        public static string MinimumLength(string name)
        {
            return name + " min {MinLength} karakter olmalıdır.";
        }
        public static string InclusiveBetween(string name)
        {
            return name + " en az {MinLength} karakter ve en çok {MaxLength} karakter olmalıdır.";
        }
        public static string GreaterThan(string name)
        {
            return name + " {ComparisonValue}'den daha buyuk  olmalıdır.";
        }
        public static string LessThan(string name)
        {
            return name + " {ComparisonValue}'den daha küçük olmalıdır.";
        }
        public static string IsInEnum(string name)
        {
            return "Lütfen " + name + " seçiniz.";
        }
        public string ErrorCode(string validationTitle, int CodeNo)
        {
            return validationTitle + CodeNo;
        }
    }
}
