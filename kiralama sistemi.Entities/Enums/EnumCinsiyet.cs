using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralamaSistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum EnumCinsiyet
    {
        Erkek = 1,
        Kadin=2,
        
    }
}
