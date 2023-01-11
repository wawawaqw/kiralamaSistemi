using System.Globalization;

namespace kiralamaSistemi.Entities.Extensions
{
    public static class EnumExtensions
    {
        private static object? GetEnumAttribute<T, TAttributeType>(this T? value)
            where T : Enum
            where TAttributeType : IAttribute
        {
            if (value == null)
            {
                return null;
            }
            Type type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null)
            {
                return null;
            }
            var field = type.GetField(name);
            var customAttribute = field?.GetCustomAttributes(typeof(TAttributeType), false)?.FirstOrDefault();
            return ((TAttributeType?)customAttribute)?.Value;
        }

        public static string? GetEnumTitle<T>(this T value)
            where T : Enum
        {
            return value.GetEnumAttribute<Enum, TitleAttribute>() as string;
        }

        public static T? StringToEnum<T>(string e)
            where T : Enum
        {
            try
            {
                var t = GetEnum<T>(i => i.Title?.ToLower(new CultureInfo("en-US")) == e.ToLower(new CultureInfo("en-US"))
                                || i.Name?.ToLower(new CultureInfo("en-US")) == e.ToLower(new CultureInfo("en-US"))
                                || i.Id.ToString(new CultureInfo("en-US")) == e.ToLower(new CultureInfo("en-US")));
                return t;
            }
            catch (Exception?)
            {
                return default;
            }
        }

        public static T? GetEnum<T>(Func<EnumEntity, bool> predicate)
           where T : Enum
        {
            if (predicate == null) return default;
            var tt = EnumToList<T>(predicate);
            var ee = tt?.FirstOrDefault()?.Enum;
            return (T?)(EnumToList<T>(predicate)?.FirstOrDefault()?.Enum ?? default);
        }

        public static List<EnumEntity>? EnumToList<T>(Func<EnumEntity, bool>? predicate = null)
            where T : Enum
        {
            var list = ((T[])Enum.GetValues(typeof(T))).Select(c =>
            new EnumEntity()
            {
                Id = Convert.ToInt32(c),
                Name = c.ToString(),
                Title = c.GetEnumTitle(),
                Enum = c
            });

            if (predicate != null) list = list.Where(predicate);

            return list?.ToList();
        }

    }
    public class EnumEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Title { get; set; }
        public object? Enum { get; set; }

    }
}
