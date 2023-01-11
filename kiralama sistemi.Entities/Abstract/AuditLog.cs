using kiralamaSistemi.Entities.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kiralama_sistemi.Entities.Abstract
{
    [JsonObject]
    [Serializable]
    public class AuditLog
    {
        public EnumLogEvent LogEvent { get; set; }
        public object? NewObjLog { get; set; }
        public object? OldObjLog { get; set; }
        public Dictionary<string, object?>? OldValues { get; set; }
        public Dictionary<string, object?>? NewValues { get; set; }
    };

    public static class AuditLogExtensions
    {
        public static Dictionary<string, T?>? GetLogObject<T>(this List<AuditLog> auditLog, EnumLogEvent logEvent = EnumLogEvent.Create, bool newValues = true)
        {
            if (logEvent == EnumLogEvent.Delete)
            {
                newValues = false;
            }
            return (newValues ?
            auditLog?.FirstOrDefault(i => i.LogEvent == logEvent && i.NewObjLog is T)?.NewValues :
            auditLog?.FirstOrDefault(i => i.LogEvent == logEvent && i.OldObjLog is T)?.OldValues) as Dictionary<string, T?>;
        }

        public static List<Dictionary<string, T?>?>? GetLogObjects<T>(this List<AuditLog> auditLog, EnumLogEvent logEvent = EnumLogEvent.Create, bool newValues = true)
        {
            if (logEvent == EnumLogEvent.Delete)
            {
                newValues = false;
            }
            return newValues ?
            auditLog?.Where(i => i.LogEvent == logEvent && i.NewObjLog is T)?.Select(i => i.NewValues as Dictionary<string, T?>)?.ToList() :
            auditLog?.Where(i => i.LogEvent == logEvent && i.OldObjLog is T)?.Select(i => i.OldValues as Dictionary<string, T?>)?.ToList();
        }

        public static (Dictionary<string, T?>?, Dictionary<string, T?>?) Dic4Editing<T>(this List<AuditLog> auditLog)
        {
            return
            (auditLog?.FirstOrDefault(i => i.LogEvent == EnumLogEvent.Create && i.NewObjLog is T)?.NewValues as Dictionary<string, T?>,
            auditLog?.FirstOrDefault(i => i.LogEvent == EnumLogEvent.Delete && i.OldObjLog is T)?.OldValues as Dictionary<string, T?>);
        }

        public static (List<Dictionary<string, T?>?>?, List<Dictionary<string, T?>?>?) Dics4Editing<T>(this List<AuditLog> auditLogsa)
        {
            return
            (auditLogsa.Where(i => i.LogEvent == EnumLogEvent.Create && i.NewObjLog is T)?.Select(i => i.NewValues as Dictionary<string, T?>)?.ToList(),
            auditLogsa.Where(i => i.LogEvent == EnumLogEvent.Delete && i.OldObjLog is T)?.Select(i => i.OldValues as Dictionary<string, T?>)?.ToList());
        }

    }
}
