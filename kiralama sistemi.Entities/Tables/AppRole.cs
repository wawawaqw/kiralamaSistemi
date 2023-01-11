using kiralamaSistemi.Entities.Abstract;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace kiralamaSistemi.Entities.Tables;

public class AppRole : IdentityRole<int>
{

    public bool Locked { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? CreatedById { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedById { get; set; }
    public string? ModifiedByName { get; set; }
    [NotMapped]
    public LogInfo LogInfo { get; set; }
    public virtual List<AppUserRole> AppRoleUsers { get; set; }

}
