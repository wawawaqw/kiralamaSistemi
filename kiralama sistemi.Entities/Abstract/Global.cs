using Microsoft.Extensions.Caching.Memory;

namespace kiralamaSistemi.Entities.Abstract
{
    public class Global
    {

        public static readonly string ProjectName = "Havuz";

        public static readonly string FrontEndDomain = "https://ebasvuru.stemegitimciler.org";

        public static readonly string Domain = "https://site.param.net";

        public static DateTimeOffset IdentityLockoutEndDateOffset = new(DateTime.Now.AddMinutes(5));

        public static MemoryCacheEntryOptions MemoryCacheLoginBanEndDate = new()
        {
            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddDays(1)),
        };

        public static MemoryCacheEntryOptions MemoryCacheLoginEndDate = new()
        {
            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(30)),
            SlidingExpiration = TimeSpan.FromMinutes(15),
        };


        public static class JWTKeys
        {
            public static readonly string Secret = "JWT:Secret";
            public static readonly string ValidAudience = "JWT:ValidAudience";
            public static readonly string ValidIssuer = "JWT:ValidIssuer";
        }

        public static class TokenKeys
        {
            public static readonly string TokenSecret = "TokenSecret";
            public static readonly string GuidKey = "GuidKey";
            public static readonly string LoginProvider = "LoginProvider";
            public static readonly string BilimAPI = "BilimAPI";
            public static readonly string Claims = "Claims";
            public static DateTimeOffset GecerlilikSure = new(DateTime.Now.AddDays(1));
            public static TimeSpan SimdidenGecerlilikSure = TimeSpan.FromDays(1);
            public static TimeSpan KayanGecerlilik = TimeSpan.FromMinutes(15);
            public static MemoryCacheEntryOptions ClaimSaklamaOptions = new()
            {
                // Claim bilgileri RAM'de saklanacağı süre
                //AbsoluteExpiration = GecerlilikSure,
                //AbsoluteExpirationRelativeToNow = SimdidenGecerlilikSure,
                SlidingExpiration = KayanGecerlilik,
            };
        }

        public static class ClaimTypes
        {
            public static readonly string AdSoyad = "AdSoyad";
            public static readonly string UserId = "UserId";
            public static readonly string LoginId = "LoginId";
            public static readonly string CurrencyId = "CurrencyId";
            public static readonly string SubeId = "SubeId";
            public static readonly string VezneAtamaId = "VezneAtamaId";
            public static readonly string RoleName = "RoleName";
            public static readonly string LoginScret = "LoginScret";
            public static readonly string UserMudurlukId = "UserMudurlukId";
            public static readonly string UserMudurlukIds = "UserMudurlukIds";
            public static readonly string Konum = "Konum";
        }

        public static class Files
        {
            public static readonly string xmlErrorFileUrl = Folders.errorsFolder + "error";
        }

        public static class Folders
        {
            //Virtual Folders
            public static readonly string errorsFolder = "Errors/";
            public static readonly string ContentFolder = "Content/";
            public static readonly string TempFolder = ContentFolder + "Temp/";
            public static readonly string Contents = ContentFolder + "Contents/";
            public static readonly string Blogs = ContentFolder + "Blogs/";
            public static readonly string Contacts = ContentFolder + "Contacts/";
            public static readonly string Mansets = ContentFolder + "Mansets/";
            public static readonly string Users = ContentFolder + "Users/";
            public static readonly string Elements = ContentFolder + "Elements/";

            public static readonly string[] FirmaFolders = new string[] {
                ContentFolder,
                TempFolder,
                Contents,
                Blogs,
                Contacts,
                Mansets,
                Users,
                Elements,
                errorsFolder
            };

            //Physcial Folders
            public static string GetPhysicalPath(string sFolder)
            {
                if (String.IsNullOrWhiteSpace(sFolder))
                {
                    return Path.Combine(Directory.GetCurrentDirectory());
                }
                var tt = Path.Combine(Directory.GetCurrentDirectory(), (sFolder).Replace("/", "\\"));
                return Path.Combine(Directory.GetCurrentDirectory(), (sFolder).Replace("/", "\\"));
            }

            //Create File Folders
            public static void CreateFolders()
            {
                //Content
                var path = GetPhysicalPath(ContentFolder);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FirmaFolders.ToList().ForEach(folder =>
                {
                    path = GetPhysicalPath(folder);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                });
            }
        }

        public class IdentityKeys
        {
            public static readonly int CapcthaOnFailLoginCount = 3;                                                               // Capthca doğrulama kodu kaç denemeden sonra açılacak ?
            public static readonly int IdentityLockedOnFailCount = 6;                                                             // Login kaç seferden sonra kilitlenecek ?
            public static readonly int BanOnFailLoginCount = 10;                                                                  // IP ban kaç süreden sonra etkin olacak ?
            public static readonly int BanDurationMinute = 3;                                                                     // Ip ban toplam banlama süresi (dakika)
            public static readonly string InvalidLoginsCacheKey = "invalidLogins";                                                // Banlanmış IP listesi cache adresi

        }

        public static class SMTP
        {
            public static readonly string Server = "mail.pigmentsoft.com";
            public static readonly string User = "bilgi@pigmentsoft.com";
            public static readonly string Password = "UZmn6327.";
        }

        public static class PermissionList
        {
            public static class BannerPermissions
            {

                public const string List = "banner#$#list";
                public const string Detail = "banner#$#detail";
                public const string Create = "banner#$#create";
                public const string Edit = "banner#$#edit";
                public const string Delete = "banner#$#delete";
            }

            public static class ContentPermissions
            {
                public const string CategoryList = "content-category#$#list";
                public const string CategoryDetail = "content-category#$#detail";
                public const string CategoryCreate = "content-category#$#create";
                public const string CategoryEdit = "content-category#$#edit";
                public const string CategoryDelete = "content-category#$#delete";

                public const string List = "content#$#list";
                public const string Detail = "content#$#detail";
                public const string Create = "content#$#create";
                public const string Edit = "content#$#edit";
                public const string Delete = "content#$#delete";
            }

            public static class BlogPermissions
            {
                public const string CategoryList = "blog-category#$#list";
                public const string CategoryDetail = "blog-category#$#detail";
                public const string CategoryCreate = "blog-category#$#create";
                public const string CategoryEdit = "blog-category#$#edit";
                public const string CategoryDelete = "blog-category#$#delete";

                public const string List = "blog#$#list";
                public const string Detail = "blog#$#detail";
                public const string Create = "blog#$#create";
                public const string Edit = "blog#$#edit";
                public const string Delete = "blog#$#delete";
            }

            public static class UserPermissions
            {
                public const string RoleList = "role#$#list";
                public const string RoleDetail = "role#$#detail";
                public const string RoleCreate = "role#$#create";
                public const string RoleEdit = "role#$#edit";
                public const string RoleDelete = "role#$#delete";

                public const string List = "user#$#list";
                public const string Detail = "user#$#detail";
                public const string Create = "user#$#create";
                public const string Edit = "user#$#edit";
                public const string Delete = "user#$#delete";
            }
        }
    }
}
