using System.ComponentModel.DataAnnotations.Schema;

namespace kiralamaSistemi.Entities.Abstract
{
    [NotMapped]
    public class LogInfo
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? CustomTitle { get; set; }
        public int? LoginId { get; set; }
    }
}
