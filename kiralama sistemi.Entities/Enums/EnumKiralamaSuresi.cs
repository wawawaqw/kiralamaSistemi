using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralama_sistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum EnumKiralamaSuresi
    {
        OnIki=1,
        BirGun=2,
        BirHafta=3,
        BirAy=4,
    }
}
