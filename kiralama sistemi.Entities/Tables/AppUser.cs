using Microsoft.AspNetCore.Identity;
using kiralamaSistemi.Entities.Abstract;

namespace kiralamaSistemi.Entities.Tables
{
    public class AppUser: IdentityUser<int>
    {

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public bool State { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedById { get; set; }
        public string? ModifiedByName { get; set; }
        public bool IsDeleted { get; set; }
        public LogInfo? LogInfo { get; set; }
        public Musteri Musteri { get; set; }
        public List<Login> Logins { get; set; }
        public List<AppUserRole> AppUserRoles { get; set; }
    }
}
