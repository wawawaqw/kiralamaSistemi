using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace kiralamaSistemi.DataAccess.Abstract
{
    public interface IAccountRepository : IRepository<AppUser>
    {
        Task LoginByEmailAsync(string userName, string password, Login login);
        Task<SignInResult> LoginByEmailAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure, Login login);
        Task LogoutAsync(ClaimsPrincipal principal);
        Task ResetPasswordEmailAsync(string email, string? domain = null);
        Task ConfirmEmailAsync(int id, string token);
        Task VerifyUserResetTokenAsync(AppUser user, string token);
        Task ResetPasswordAsync(string name, string token, string password);
        Task SignUpAsync(AppUser user, string password);
        Task CreateUserAsync(AppUser user, string password, string? domain = null);
        Task UpdateUserAsync(AppUser user);
        Task Profil(AppUser user);
        Task UpdateUserStateAsync(bool state, int id, LogInfo log);
    }
}
