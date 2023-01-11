using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Extensions;
using System.Security.Claims;

namespace kiralamaSistemi.DataAccess.Sevices
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(Global.ClaimTypes.UserId).TryToInt();
        }

        public static string GetLoginScret(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(Global.ClaimTypes.LoginScret);
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static int? GetUserLoginId(this ClaimsPrincipal principal)
        {
            var loginId = principal.FindFirstValue(Global.ClaimTypes.LoginId);

            return loginId.TryToInt();
        }

        public static string GetGuidKey(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(Global.TokenKeys.GuidKey);
        }

        public static string GetAdSoyad(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(Global.ClaimTypes.AdSoyad);
        }

        //public static int GetSubeId(this ClaimsPrincipal principal)
        //{
        //    return principal.FindFirstValue(Global.ClaimTypes.SubeId).TryToInt();
        //}

        //public static int GetVezneAtamaId(this ClaimsPrincipal principal)
        //{
        //    return principal.FindFirstValue(Global.ClaimTypes.VezneAtamaId).TryToInt();
        //}

        //public static bool IsMerkez(this ClaimsPrincipal principal)
        //{
        //    var konum = EnumExtensions.StringToEnum<EnumUserKonum>(principal.FindFirstValue(Global.ClaimTypes.Konum));
        //    return konum is EnumUserKonum.Mudurluk;

        //}

        public static int GetMudurlukId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(Global.ClaimTypes.UserMudurlukId).TryToInt();
        }

        public static List<int>? GetMudurlukIds(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(Global.ClaimTypes.UserMudurlukIds)?.Split(",").Select(i => i.TryToInt()).ToList();
        }

        public static void Creating<T>(this T entity, LogInfo logInfo)
        where T : BaseEntity
        {
            entity.CreatedById = logInfo.UserId;
            entity.CreatedByName = logInfo.UserName;
            entity.CreatedDate = DateTime.Now;
            entity.LogInfo = logInfo;
        }

        public static T CreatingEntity<T>(this T entity, LogInfo logInfo)
        where T : BaseEntity
        {
            entity.Creating(logInfo);
            return entity;
        }

        public static void Modifiedation<T>(this T entity, LogInfo logInfo)
        where T : BaseEntity
        {
            entity.ModifiedById = logInfo.UserId;
            entity.ModifiedByName = logInfo.UserName;
            entity.ModifiedDate = DateTime.Now;
            entity.LogInfo = logInfo;
        }

        public static T ModifiedationEntity<T>(this T entity, LogInfo logInfo)
        where T : BaseEntity
        {
            entity.Modifiedation(logInfo);
            return entity;
        }

        public static LogInfo GetLogInfo(this ClaimsPrincipal principal, string? customTitle = null)
        {
            return new LogInfo
            {
                CustomTitle = customTitle,
                LoginId = principal.GetUserLoginId(),
                UserId = principal.GetUserId(),
                UserName = principal.GetUserName(),
            };
        }
    }
}
