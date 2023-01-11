using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.Entities.Tables
{
    public class LogModuleMap
    {
        public int LogId { get; set; }
        public Log Log { get; set; }
        public int DataId { get; set; }
        public EnumModules Module { get; set; }
        public string? Title { get; set; }
    }
}
