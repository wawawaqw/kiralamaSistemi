using System.ComponentModel.DataAnnotations.Schema;

namespace kiralamaSistemi.Entities.Abstract
{
    public interface IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedById { get; set; }
        public string ModifiedByName { get; set; }
        public LogInfo LogInfo { get; set; }
    }
    public abstract class BaseEntity : IBaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedById { get; set; }
        public string? ModifiedByName { get; set; }
        [NotMapped]
        public LogInfo? LogInfo { get; set; }
    }
}
