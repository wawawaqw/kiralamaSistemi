
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralamaSistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum EnumLogEvent
    {
        //  [Title("Ekleme")]
        Create = 10,
        // [Title("Düzenleme")]
        Update = 20,
        //  [Title("Silme")]
        Delete = 30,
        //[Title("Giriş")]
        Login = 40,
        // [Title("Çıkış")]
        Logout = 50,
    }
}
