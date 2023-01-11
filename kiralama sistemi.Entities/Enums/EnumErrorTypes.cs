
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kiralamaSistemi.Entities.Enums
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum EnumErrorTypes
    {
        //[Title("danger")]
        danger = 1,
      //  [Title("success")]
        success = 2,
        //[Title("info")]
        info = 3,
       // [Title("warning")]
        warning = 4,
    }
}
