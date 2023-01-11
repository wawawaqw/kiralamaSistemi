using kiralamaSistemi.Entities.Enums;

namespace kiralamaSistemi.Entities.Tables
{
    public class Log
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public EnumModules? Module { get; set; }
        public int DataId { get; set; }
        public EnumLogEvent Event { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int? LoginId { get; set; }
        public virtual Login Login { get; set; }
        public virtual List<LogModuleMap> LogModuleMaps { get; set; }

    }
}
