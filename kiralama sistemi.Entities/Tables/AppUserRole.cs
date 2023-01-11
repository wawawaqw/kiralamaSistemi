
using Microsoft.AspNetCore.Identity;

namespace kiralamaSistemi.Entities.Tables
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
