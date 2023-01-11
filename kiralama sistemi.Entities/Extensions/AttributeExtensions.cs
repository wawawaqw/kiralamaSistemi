using System.ComponentModel.DataAnnotations;


namespace kiralamaSistemi.Entities.Extensions
{

    public interface IAttribute
    {
        public object Value { get; }
    }

    [AttributeUsage(AttributeTargets.All)]

    public class IgnoreAttribute : Attribute, IAttribute
    {
        public IgnoreAttribute(bool ignored = true)
        {
            this.Ignored = ignored;
        }
        public bool Ignored = false;
        public object Value => Ignored;
    }


    [AttributeUsage(AttributeTargets.All)]
    public class DefaultAttribute : Attribute, IAttribute
    {
        public DefaultAttribute(bool isDefault = true)
        {
            IsDefault = isDefault;
        }
        public bool IsDefault = false;
        public object Value => IsDefault;
    }


    [AttributeUsage(AttributeTargets.All)]
    public class LogNameAttribute : Attribute, IAttribute
    {
        public LogNameAttribute(string name)
        {
            this.Name = name;
        }
        public string Name;
        public object Value => Name;
    }


    [AttributeUsage(AttributeTargets.All)]
    public class WidthAttribute : Attribute, IAttribute
    {
        public WidthAttribute(int title)
        {
            this.Width = title;
        }
        public int Width { get; set; }

        public object Value => Width;
    }


    [AttributeUsage(AttributeTargets.All)]
    public class TitleAttribute : Attribute, IAttribute
    {
        public TitleAttribute(string title)
        {
            this.Title = title;
        }
        public string Title { get; set; }

        public object Value => Title;
    }


    [AttributeUsage(AttributeTargets.Method)]
    public class RequredAuthorizeAttribute : Attribute
    {

    }


    public class ValidationTypeAttribute : Attribute, IAttribute
    {
        public ValidationTypeAttribute(Type type)
        {
            this.Type = type;
        }
        public Type Type { get; set; }

        public object Value => Type;
    }


    [AttributeUsage(AttributeTargets.All)]
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(int order)
        {
            this.Order = order;
        }
        public int Order { get; set; }
    }



    [AttributeUsage(AttributeTargets.All)]
    public class AuthPermissonAttribute : Attribute
    {
        public AuthPermissonAttribute(string name, string description = null)
        {
            this.Name = name;
            this.Description = description;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class LabelAttribute : Attribute
    {
        public LabelAttribute(string name, string description = null)
        {
            this.Name = name;
            this.Description = description;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class InputMapAttribute : Attribute
    {
        public InputMapAttribute(bool requiredName = true, bool requiredPass = true, bool requiredApiKey = false)
        {
            this.RequiredName = requiredName;
            this.RequiredPass = requiredPass;
            this.RequiredAPIKey = requiredApiKey;
        }
        public bool RequiredName { get; set; }
        public bool RequiredPass { get; set; }
        public bool RequiredAPIKey { get; set; }
    }

    public class CustomRequiredAttr : RequiredAttribute
    {
        public CustomRequiredAttr(string Error)
        {
            this.ErrorMessage = Error;
        }
        public override object TypeId => base.TypeId;

        public override bool RequiresValidationContext => base.RequiresValidationContext;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }

        public override bool IsValid(object value)
        {

            if (value == null)
            {
                return false;
            }

            return false;
        }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext.MemberName = "danger";

            return base.IsValid(value, validationContext);
        }

        public class DecimalUnitAttribute : Attribute
        {
            public DecimalUnitAttribute(string decimalUnit)
            {
                this.DecimalUnit = decimalUnit;
            }
            public string DecimalUnit { get; set; }
        }



    }
}
