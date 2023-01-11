using Newtonsoft.Json;
using System.Reflection;

namespace kiralamaSistemi.Entities.Extensions
{
    public static class TypeExtensions
    {
        public static object? GetPropAttribute<AttributeType>(this Type type, string propName)
            where AttributeType : IAttribute
        {
            try
            {
                if (string.IsNullOrWhiteSpace(propName))
                {
                    return null;
                }
                var customAttribute = type.GetProperty(propName)?.GetCustomAttributes(typeof(AttributeType), true).FirstOrDefault();
                return ((AttributeType?)customAttribute)?.Value;
            }
            catch (Exception?)
            {
                return default;
            }
        }

        public static object? GetAttribute<AttributeType>(this Type type)
            where AttributeType : IAttribute
        {
            var customAttribute = type.GetCustomAttributes(typeof(AttributeType), false).FirstOrDefault();
            return ((AttributeType?)customAttribute)?.Value;
        }

        public static bool IsIgnored(this Type type, string propName)
        {
            return type.GetPropAttribute<IgnoreAttribute>(propName) as bool? ?? false;
        }

        public static bool IsDefault(this Type type, string propName)
        {
            return type.GetPropAttribute<DefaultAttribute>(propName) as bool? ?? false;
        }

        public static string? GetPropTitle(this Type type, string propName, bool defaultValue = true)
        {
            return type.GetPropAttribute<TitleAttribute>(propName) as string ?? (defaultValue ? propName : null);
        }

        public static int? GetPropWidth(this Type type, string propName)
        {
            return type.GetPropAttribute<WidthAttribute>(propName) as int?;
        }

        public static string GetPropLogName(this Type type, string propName)
        {
            return type.GetPropAttribute<LogNameAttribute>(propName) as string ?? propName;
        }

        public static string GetJsonPropName(this Type type, string propName)
        {
            if (type == null || string.IsNullOrWhiteSpace(propName))
            {
                return propName;
            }
            var d = (type.GetType().GetProperty(propName)?.GetCustomAttribute(typeof(JsonPropertyAttribute), true) as JsonPropertyAttribute)?.PropertyName ?? propName;
            return d;
        }

        public static TResult? ToObject<TSource, TResult>(this TSource source, Func<TSource, TResult> selector)
        {
            return new List<TSource>() { source }.Select(selector).FirstOrDefault();
        }


        public static TResult? CopyTo<TResult>(this TResult source)
        {
            try
            {
                TResult? result = (TResult?)Activator.CreateInstance(typeof(TResult));
                var sourseProps = source?.GetType().GetProperties().ToList();
                result?.GetType().GetProperties().ToList().ForEach(i =>
                {
                    i.SetValue(result, sourseProps?.Find(j => j.Name == i.Name)?.GetValue(source));
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static TResult CopyTo<TSource, TResult>(this TSource source)
        {
            try
            {
                TResult result = (TResult)Activator.CreateInstance(typeof(TResult));
                var sourseProps = source.GetType().GetProperties().ToList();
                result.GetType().GetProperties()
                    .Where(i => sourseProps.Select(j => (j.Name, j.PropertyType)).Contains((i.Name, i.PropertyType)))
                    .ToList().ForEach(i => i.SetValue(result, sourseProps.Find(j => j.Name == i.Name).GetValue(result)));
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<T> ObjectToList<T>(this T element)
        {
            return new List<T>() { element };
        }
    }
}
