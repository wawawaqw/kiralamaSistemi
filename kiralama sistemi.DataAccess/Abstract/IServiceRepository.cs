using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace kiralamaSistemi.DataAccess.Abstract
{
    public interface IServiceRepository
    {
        Result CheckAndStoreLoginIp(string ip);
        bool IsVisibleCaptcha(string ip);
        void RemoveFromLogins(string? ip);
        Result IsIpBanned(string ip);
        Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act);
        Task<bool> SetPermissionsByRoleIdAsync(int id, IEnumerable<int> permissionIds, LogInfo logInfo);
    }
}
