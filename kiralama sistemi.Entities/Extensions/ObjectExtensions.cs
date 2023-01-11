using kiralamaSistemi.Entities.Abstract;
using System.Collections;
using System.Reflection;

namespace kiralamaSistemi.Entities.Extensions
{
    public static class ObjectExtensions
    {
        public static object? GetObjectPropAttribute<T, AttributeType>(this T? value, string propName)
            where T : class  
            where AttributeType : IAttribute
        {
            return value?.GetType().GetPropAttribute<AttributeType>(propName);
        }

        public static object? GetObjectAttribute<T, AttributeType>(this T? value)
            where T : class
            where AttributeType : IAttribute
        {
            return value?.GetType().GetAttribute<AttributeType>();
        }
        public static object? GetProperty<T>(this T? value, Type PropType, string propName)
        {
            try
            {
                var prop = value?.GetType().GetProperty(propName);
                if (prop == null) return null;
                return prop?.PropertyType == PropType ? prop.GetValue(value) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Type? GetValidationType<T>(this T value)
            where T : class
        {
            return value.GetObjectAttribute<T, ValidationTypeAttribute>() as Type;
        }

        public static string GetPropTitle<T>(this T value, string propName)
            where T : class
        {
            return value.GetObjectPropAttribute<T, TitleAttribute>(propName) as string ?? propName;
        }

        public static string? TryToString(this Object s)
        {
            return s?.ToString();
        }

        public static string? TryToEmptyString(this Object? o)
        {
            return o == null ? string.Empty : o.ToString();
        }

        public static void RemoveObjectWhiteSpaces(this object entity, Type? declaringType = null)
        {
            entity.GetType().GetProperties()?.Where(p => p.GetValue(entity) != null)?.ToList()?.ForEach(p =>
            {
                if (p.PropertyType == typeof(string) && p.GetValue(entity) is string value)
                {
                    p.SetValue(entity, value.RemoveWhiteSpaces());
                }
                else if (p.PropertyType != declaringType
                        && p.GetValue(entity) is object obj
                        && !p.PropertyType.IsEnum
                        && (p.PropertyType.UnderlyingSystemType?.FullName?.Contains(Global.ProjectName) ?? false)
                        && p.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).Any())
                {
                    if (p.PropertyType.AssemblyQualifiedName?.Contains("Collection", StringComparison.CurrentCultureIgnoreCase) ?? false)
                    {
                        var dv = (IList)obj;
                        if (dv?.Count > 0 && dv[0]?.GetType() != declaringType)
                        {
                            for (int i = 0; i < dv.Count; i++)
                            {
                                dv[i]?.RemoveObjectWhiteSpaces(p.DeclaringType);
                            }
                        }
                    }
                    else
                    {
                        obj.RemoveObjectWhiteSpaces(p.DeclaringType);
                    }
                }
            });
        }
    }
}
