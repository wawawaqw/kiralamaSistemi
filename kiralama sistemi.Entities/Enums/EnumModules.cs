
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralamaSistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum EnumModules
    {
        // [Title("User")]
        User = 10,
        //   [Title("Musteri")]
        Turnike = 20,
        // [Title("Araba")]
        Araba = 30,
        // [Title("Tarife")]
        Tarife = 40,
        // [Title("Kiralama")]
        Kiralama = 50,
        //  [Title("Role")]
        Role = 110,
    }
}

