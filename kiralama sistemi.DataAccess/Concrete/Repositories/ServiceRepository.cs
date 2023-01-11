using kiralama_sistemi.Entities.Abstract;
using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Abstract;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace kiralama_sistemi.DataAccess.Concrete.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly IMemoryCache _cache;
        public ServiceRepository(IMemoryCache cache)
        {
            _cache = cache;
        }
        public Result CheckAndStoreLoginIp(string ip)
        {
            try
            {
                var invalidLogins = GetInvalidLogins();
                if (invalidLogins == null)
                {
                    invalidLogins = new List<InvalidLogin>
                    {
                        new InvalidLogin()
                        {
                            Date = DateTime.Now,
                            AttemptCount = 1,
                            Ip = ip
                        }
                    };
                    SetInvalidLogins(invalidLogins);
                    return new Result(true);
                }
                var current_Login = invalidLogins.FirstOrDefault(i => i.Ip == ip);
                if (current_Login == null)
                {
                    current_Login = new InvalidLogin()
                    {
                        Date = DateTime.Now,
                        AttemptCount = 1,
                        Ip = ip
                    };
                    invalidLogins.Add(current_Login);
                    SetInvalidLogins(invalidLogins);
                    return new Result(true);
                }
                //Check for banned
                if (current_Login.IsBanned == true)
                {
                    int left_minutes = (int)(DateTime.Now - current_Login.Date).TotalMinutes;
                    if (left_minutes > Global.IdentityKeys.BanDurationMinute)
                    {
                        current_Login.IsBanned = false;
                        current_Login.AttemptCount = 1;
                        SetInvalidLogins(invalidLogins);
                        return new Result(true);
                    }
                    else
                    {
                        TimeSpan span = (current_Login.Date.AddMinutes(Global.IdentityKeys.BanDurationMinute) - DateTime.Now);
                        var remain_date = String.Format("{0} dakika, {1} saniye", span.Minutes, span.Seconds);
                        return new Result(ErrorProvider.Auth.Banned.DescriptionWithFormat(remain_date));
                    }
                }

                if (current_Login.AttemptCount < Global.IdentityKeys.BanOnFailLoginCount)
                    current_Login.AttemptCount++;
                else
                    current_Login.IsBanned = true;

                current_Login.Date = DateTime.Now;
                SetInvalidLogins(invalidLogins);

                if (current_Login.IsBanned == true)
                {
                    TimeSpan span = (current_Login.Date.AddMinutes(Global.IdentityKeys.BanDurationMinute) - DateTime.Now);
                    var remain_date = String.Format("{0} dakika, {1} saniye", span.Minutes, span.Seconds);
                    return new Result(ErrorProvider.Auth.MaliciousLogin.DescriptionWithFormat(remain_date));
                }
                return new Result(true);
            }
            catch (OzelException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(ServiceRepository)} - {nameof(CheckAndStoreLoginIp)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }


        public Task<bool> GetMenuItemsAsync(ClaimsPrincipal ctx, string ctrl, string act)
        {
            throw new NotImplementedException();
        }

        public Result IsIpBanned(string ip)
        {
            try
            {
                var invalidLogins = GetInvalidLogins();
                if (invalidLogins != null)
                {
                    var current_Login = invalidLogins.Where(i => i.Ip == ip).FirstOrDefault();
                    // Current invalidLogin is existes
                    if (current_Login != null)
                    {
                        if (current_Login.IsBanned)
                        {
                            //int left_minutes = (int)(DateTime.Now - current_Login.Date).TotalMinutes;
                            TimeSpan span = (current_Login.Date.AddMinutes(Global.IdentityKeys.BanDurationMinute) - DateTime.Now);
                            if (span.CompareTo(TimeSpan.Zero) <= 0)
                            {
                                RemoveFromLogins(ip);
                                return new Result(true);
                            }
                            var remain_date = String.Format("{0} dakika, {1} saniye", span.Minutes, span.Seconds);
                            return new Result(ErrorProvider.Auth.MaliciousLogin.DescriptionWithFormat(remain_date));
                        }
                    }
                }
                return new Result(true);
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(ServiceRepository)} - {nameof(IsIpBanned)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public bool IsVisibleCaptcha(string ip)
        {
            try
            {
                var invalidLogins = GetInvalidLogins();
                if (invalidLogins != null)
                {
                    var current_Login = invalidLogins.Where(i => i.Ip == ip).FirstOrDefault();
                    // Current invalidLogin is existes
                    if (current_Login != null)
                    {
                        if (current_Login.AttemptCount >= Global.IdentityKeys.CapcthaOnFailLoginCount)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(ServiceRepository)} - {nameof(IsVisibleCaptcha)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public void RemoveFromLogins(string? ip)
        {
            try
            {
                var invalidLogins = GetInvalidLogins();
                if (invalidLogins != null)
                {
                    var current_Login = invalidLogins.Where(i => i.Ip == ip).FirstOrDefault();
                    // Current invalidLogin is existes
                    if (current_Login != null)
                    {
                        invalidLogins.RemoveAll(i => i.Ip == ip);
                        SetInvalidLogins(invalidLogins);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(ServiceRepository)} - {nameof(RemoveFromLogins)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        public Task<bool> SetPermissionsByRoleIdAsync(int id, IEnumerable<int> permissionIds, LogInfo logInfo)
        {
            throw new NotImplementedException();
        }


        private List<InvalidLogin> GetInvalidLogins()
        {
            try
            {
                _cache.TryGetValue(Global.IdentityKeys.InvalidLoginsCacheKey, out List<InvalidLogin> invalidLogins);
                return invalidLogins;
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(ServiceRepository)} - {nameof(GetInvalidLogins)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }

        private void SetInvalidLogins(List<InvalidLogin> invalidLogins)
        {
            try
            {
                if (invalidLogins == null || invalidLogins.Count <= 0)
                {
                    return;
                }
                _cache.Set(Global.IdentityKeys.InvalidLoginsCacheKey, invalidLogins, Global.MemoryCacheLoginBanEndDate);
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(DataAccess)} - {nameof(Repositories)} - {nameof(ServiceRepository)} - {nameof(SetInvalidLogins)}"));
                throw new OzelException(ErrorProvider.DataAccessHatasi);
            }
        }
    }
}
