using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Extensions;
using kiralamaSistemi.Entities.Enums;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace kiralamaSistemi.DataAccess.Concrete.Repositories
{
    public class AccountRepository : GenericRepository<AppUser>, IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IServiceRepository _serviceRepository;
        private readonly Abstract.IRepository<Log> _logService;
        //private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        //private readonly IRoleRepository _roleRepository;
        private readonly IMemoryCache _cache;

        public AccountRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            Abstract.IRepository<Log> logService,
            IServiceRepository serviceRepository,
            //RoleManager<AppRole> roleManager,
            IEmailSender emailSender,
            //IRoleRepository roleRepository,
            IMemoryCache cache
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logService = logService;
            _serviceRepository = serviceRepository;
            // _roleManager = roleManager;
            _emailSender = emailSender;
            //_roleRepository = roleRepository;
            _cache = cache;
        }
        public async Task<SignInResult> LoginByEmailAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure, Login login)
        {

            var user = await _userManager.Users.Where(i => i.UserName == userName).FirstOrDefaultAsync();
            if (user != null)
            {
                // is Confirmed Email ?
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    throw new OzelException(ErrorProvider.Auth.NotConfirmedEmail);
                // Check password
                var checkPass = await _userManager.CheckPasswordAsync(user, password);

                if (!user.State)
                {
                    throw new OzelException(ErrorProvider.Account.AccountIsNotActive);
                }

                if (checkPass)
                {
                    await _userManager.UpdateSecurityStampAsync(user);

                }
                // Daha önceki cookie silinsin
                await _signInManager.SignOutAsync();

                // Create Log 
                var log = new Log()
                {
                    Event = EnumLogEvent.Login,
                    UserId = user.Id,
                    Login = login,
                    Date = DateTime.Now,
                    UserName = user.UserName,
                };
                await _logService.CreateAsync(log);

                SignInResult result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);                                    //Önceki hataları girişler neticesinde +1 arttırılmış tüm değerleri 0(sıfır)a çekiyoruz.
                    await _userManager.SetLockoutEndDateAsync(user, null);                                   //Remove from cache current user ip
                    //Ip check
                    _serviceRepository.RemoveFromLogins(login.Ip);

                    var UserLogins = GetUserLogins();
                    UserLogins.Add(new LoggedUser() { UserName = user.Email, Kod = user.SecurityStamp });

                    SetUserLogins(UserLogins);

                    //Ip check
                    _serviceRepository.RemoveFromLogins(login.Ip);


                }
                else if (result.IsLockedOut)
                {
                    await _signInManager.SignOutAsync();
                    await _logService.UpdateAsync(log);
                }
                else
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    await _userManager.AccessFailedAsync(user); //Eğer ki başarısız bir account girişi söz konusu ise AccessFailedCount kolonundaki değer +1 arttırılacaktır. 
                    await _signInManager.SignOutAsync();
                    await _logService.UpdateAsync(log);
                }
                return result;
            }
            return Microsoft.AspNetCore.Identity.SignInResult.Failed;

        }
        public async Task LoginByEmailAsync(string userName, string password, Login login)
        {
            var context = new AppDbContext();
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == userName);
                if (user == null) throw new OzelException(ErrorProvider.Auth.NotFoundUser);

                // Check if User Locked
                if (await _userManager.IsLockedOutAsync(user))
                {
                    DateTimeOffset? d = await _userManager.GetLockoutEndDateAsync(user);
                    DateTime dateTime = d.HasValue ? d.Value.DateTime : Global.IdentityLockoutEndDateOffset.DateTime;
                    TimeSpan span = (dateTime - DateTime.Now);
                    var remain_date = String.Format("{0} dakika, {1} saniye", span.Minutes, span.Seconds);
                    throw new OzelException(ErrorProvider.Auth.IdentityLockedout.DescriptionWithFormat(Global.IdentityKeys.IdentityLockedOnFailCount.ToString(), remain_date));
                }

                // Check password
                if (!await _userManager.CheckPasswordAsync(user, password))
                {
                    //Kullanıcının yapmış olduğu başarısız giriş deneme adedini alıyoruz.
                    int failcount = await _userManager.GetAccessFailedCountAsync(user);

                    if (failcount >= Global.IdentityKeys.IdentityLockedOnFailCount)
                    {
                        //Eğer ki başarısız giriş denemesi 3'ü bulduysa ilgili kullanıcının hesabını kilitliyoruz.
                        await _userManager.SetLockoutEndDateAsync(user, Global.IdentityLockoutEndDateOffset);
                        var span = Global.IdentityLockoutEndDateOffset - DateTime.Now;
                        var remain_date = String.Format("{0} dakika, {1} saniye", span.Minutes, span.Seconds);
                        throw new OzelException(ErrorProvider.Auth.IdentityLockedout.DescriptionWithFormat(Global.IdentityKeys.IdentityLockedOnFailCount.ToString(), remain_date));
                    }

                    await _userManager.SetLockoutEnabledAsync(user, true);
                    await _userManager.AccessFailedAsync(user);
                    //Eğer ki başarısız bir account girişi söz konusu ise AccessFailedCount kolonundaki değer +1 arttırılacaktır. 
                    throw new OzelException(ErrorProvider.Auth.InvalidLogin);
                }

                // is Confirmed Email ?
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    throw new OzelException(ErrorProvider.Auth.NotConfirmedEmail);

                if (!user.State)
                {
                    throw new OzelException(ErrorProvider.Account.AccountIsNotActive);
                }

                await _userManager.UpdateSecurityStampAsync(user);

                // Create Log 
                var log = new Log()
                {
                    Event = EnumLogEvent.Login,
                    UserId = user.Id,
                    Login = login,
                    UserName = user.UserName,
                    Title = $"<b> {user.UserName} </b> Kullanıcısı Giriş Yaptı",
                };
                await context.Logs.AddAsync(log);

                await context.SaveChangesAsync();

                //Önceki hataları girişler neticesinde +1 arttırılmış tüm değerleri 0(sıfır)a çekiyoruz.
                await _userManager.ResetAccessFailedCountAsync(user);

                //Remove from cache current user ip
                await _userManager.SetLockoutEndDateAsync(user, null);

                _serviceRepository.RemoveFromLogins(login.Ip);

                var UserLogins = GetUserLogins();
                UserLogins.Add(new LoggedUser() { UserName = user.Email, Kod = user.SecurityStamp });

                SetUserLogins(UserLogins);

                _serviceRepository.RemoveFromLogins(login.Ip);
            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(LoginByEmailAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task LogoutAsync(ClaimsPrincipal principal)
        {
            try
            {
                var log = new Log()
                {
                    Event = EnumLogEvent.Logout,
                    LoginId = principal.GetUserLoginId(),
                    UserId = principal.GetUserId(),
                    UserName = principal.GetUserName(),
                    Title = $"<b> {principal.GetUserName()} </b> Kullanıcısı Çıkış Yaptı",
                };
                await _logService.CreateAsync(log);
                var user = await _userManager.GetUserAsync(principal);
                await _userManager.UpdateSecurityStampAsync(user);
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(LogoutAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task ResetPasswordEmailAsync(string email, string? domain = null)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Email == email);
                if (user == null)
                {
                    throw new OzelException(ErrorProvider.Auth.NotFoundUser);
                }
                // is Confirmed Email ?
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    throw new OzelException(ErrorProvider.Auth.NotConfirmedEmail);
                }
                //Generate token
                var token = await _userManager.GenerateUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword");
                //var token2 = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var d = HttpUtility.UrlEncode(token); 
                await _emailSender.SendEmailAsync(user.Email, "Parola yenileme", "Lütfen parolanızı değiştirmek için linke <a href='" + CreateCallbackURL(domain, "/ResetPassword", user.Id, token) + "' >tıklayınız.</a> ");

            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(ResetPasswordEmailAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task ConfirmEmailAsync(int id, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());//.Users.Include(i => i.Firma).Where(i => i.Id == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new OzelException(ErrorProvider.Auth.NotFoundUser);
                }

                var isConfirmEmail = await _userManager.ConfirmEmailAsync(user, token);
                if (!isConfirmEmail.Succeeded)
                {
                    throw new OzelException(ErrorProvider.Auth.InvalidToken);
                }
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(ConfirmEmailAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task VerifyUserResetTokenAsync(AppUser user, string token)
        {
            try
            {
                var verifyToken = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
                if (!verifyToken)
                {
                    throw new OzelException(ErrorProvider.Auth.InvalidToken);
                }
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(VerifyUserResetTokenAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task ResetPasswordAsync(string name, string token, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(name);
                if (user == null)
                {
                    throw new OzelException(ErrorProvider.Auth.NotFoundUser);
                }
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded)
                {
                    if (result.Errors.Any(i => i.Code == "InvalidToken"))
                    {
                        throw new OzelException(ErrorProvider.Auth.InvalidToken);
                    }
                    throw new OzelException(ErrorProvider.Auth.NotChangedPass);
                }
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(ResetPasswordAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task CreateUserAsync(AppUser user, string password, string? domain = null)
        {
            var is_exist = await _userManager.FindByNameAsync(user.UserName);
            if (is_exist != null)
            {
                throw new OzelException(ErrorProvider.Account.FailedCreateUserExistAlready);
            }
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded && !user.EmailConfirmed)
            {
                //generate token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _emailSender.SendEmailAsync(user.Email, "Hesap doğrulama", "Lütfen hesabınızı doğrulamak için linke <a href='" +
                    CreateCallbackURL(domain, "/ConfirmEmail", user.Id, token) + "' >tıklayınız.</a> ");
            }
            else if (!result.Succeeded)
            {
                throw new OzelException(ErrorProvider.Account.FailedCreateUser);
            }
        }
        public async Task UpdateUserAsync(AppUser user)
        {
            using var context = new AppDbContext();
            try
            {
                await _userManager.UpdateAsync(user);
                user.LogInfo = null;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(UpdateUserAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task Profil(AppUser user)
        {
            try
            {
                await _userManager.UpdateAsync(user);
                user.LogInfo = null;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(Profil)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task UpdateUserStateAsync(bool state, int id, LogInfo log)
        {
            try
            {
                using var context = new AppDbContext();
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.LogInfo = log;
                user.State = state;
                await _userManager.UpdateAsync(user);
                user.LogInfo = null;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(UpdateUserStateAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public async Task SignUpAsync(AppUser user, string password)
        {
            var context = new AppDbContext();
            try
            {

                var user1 = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == user.UserName);
                if (user1 == null)
                {
                    await _userManager.CreateAsync(user, password);
                }
                else
                    throw new OzelException(ErrorProvider.Auth.NotFoundUser);

            }
            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(SignUpAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        public override async Task UpdateAsync(AppUser entity, Func<AppDbContext, Task>? validation = null)
        {
            var context = new AppDbContext();
            try
            {
                var userRoles = context.AppUserRoleler.Where(i => i.UserId == entity.Id);
                context.AppUserRoleler.RemoveRange(userRoles);

                context.Users.Update(entity);
                await context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(AccountRepository)} - {nameof(UpdateAsync)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
        //Private Fonksiyonlar
        private List<LoggedUser> GetUserLogins()
        {
            _cache.TryGetValue(Global.IdentityKeys.InvalidLoginsCacheKey, out List<LoggedUser> UserLogins);
            return UserLogins ?? new List<LoggedUser>();
        }
        private void SetUserLogins(List<LoggedUser> UserLogins)
        {
            if (UserLogins == null || UserLogins.Count <= 0)
            {
                return;
            }
            _cache.Set(Global.IdentityKeys.InvalidLoginsCacheKey, UserLogins, Global.MemoryCacheLoginEndDate);
        }
        private static string CreateCallbackURL(string? domain, string url, int userId, string token)
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "userId", userId.TryToString() },
                { "token", HttpUtility.UrlEncode(token)  }
            };
            //return $"{domain ?? Global.FrontEndDomain}{url}?userId={userId}&token={token}";
            return $"{domain ?? Global.FrontEndDomain}{url}?userId={userId}&token={HttpUtility.UrlEncode(token)}";
            //return $"{domain ?? Global.FrontEndDomain}{url}/{userId}/{token}";

            //return QueryHelpers.AddQueryString(Global.FrontEndDomain + url, queryParams);
        }
    }
}
