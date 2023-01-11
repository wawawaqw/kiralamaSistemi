using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralamaSistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum EnumArabaRenk
    {
        siyah=1,
        bayaz=2,
        kirimizi=3,
        mave=4,
        sari=5,
        yesil=6
    }
}
