using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralamaSistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum  EnumArabaTur
    {

        BMW=1,
        Mersidis=2,
        Kia=3,
        Hunda=4,
        Ford=5,

    }
}
