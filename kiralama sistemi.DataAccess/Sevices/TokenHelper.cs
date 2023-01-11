using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Extensions;
using kiralamaSistemi.DataAccess.Concrete;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace kiralamaSistemi.DataAccess.Sevices
{
    public static class TokenHelper
    {
        private static IMemoryCache _cache;

        public static void TokenHelperConfigure(IMemoryCache cache) => _cache = cache;

        public static string GenerateJwtToken(JwtSettings jwtSettings, AppUser user, string GuidKey, int? loginId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(Global.ClaimTypes.UserId, user.Id.ToString() ?? ""),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(Global.ClaimTypes.LoginScret, user.SecurityStamp ?? ""),
                    new Claim(Global.ClaimTypes.AdSoyad, (user.Ad + user.Soyad) ?? ""),
                    new Claim(Global.ClaimTypes.LoginId, loginId?.ToString() ?? ""),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.ValidIssuer,
                Audience = jwtSettings.ValidAudience,
            };

            if (user.AppUserRoles?.Count > 0)
            {
                tokenDescriptor.Subject.AddClaims(user.AppUserRoles.Select(i => new Claim(ClaimTypes.Role, i.Role.Name)).ToList());
            }
          

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateTcToken(JwtSettings jwtSettings, string tc)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(Global.TokenKeys.TokenSecret, tc),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.ValidIssuer,
                Audience = jwtSettings.ValidAudience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static string GeneratetimeToken(JwtSettings jwtSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretTime);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("time", "Hakim"),
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings.ValidIssuer,
                Audience = jwtSettings.ValidAudience,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var t = tokenHandler.WriteToken(token);
            return t;
        }

        public static string? GetTcFromToken(JwtSettings appSettings, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken?.Claims?.First(x => x.Type == Global.TokenKeys.TokenSecret)?.Value;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsTokenAvailable(JwtSettings appSettings, string? token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(appSettings.SecretTime);
                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = ((JwtSecurityToken)validatedToken)
                    ?.Claims.FirstOrDefault(x => x.Type == "time")?.Value;
                //var between =  DateTime.Now.Subtract(DateTime.Parse(strtime)).TotalSeconds;
                return jwtToken == "Hakim";
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string CreateUserClaims(AppUser user)
        {
            var identity = new ClaimsIdentity();

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identity.AddClaim(new Claim(Global.ClaimTypes.UserId, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim(Global.ClaimTypes.LoginScret, user.SecurityStamp));
            identity.AddClaim(new Claim(Global.ClaimTypes.AdSoyad, user.Ad));
            identity.AddClaim(new Claim(Global.ClaimTypes.LoginId, user.Logins.LastOrDefault()?.Id.ToString() ?? string.Empty));

            var guid = Guid.NewGuid().ToString();

            _cache.Set<ClaimsIdentity>(guid, identity, Global.TokenKeys.ClaimSaklamaOptions);
            return guid;
        }

        public static ClaimsIdentity CreateUserClaims(int userId)
        {
            var context = new AppDbContext();
            var user = context.Users.Include(i => i.AppUserRoles)
                                            .ThenInclude(i => i.Role)
                                          .Include(i => i.Logins)
                                          .FirstOrDefault(i => i.Id == userId);

            if (user == null) throw new OzelException(ErrorProvider.Auth.NotFoundUser);

            var identity = new ClaimsIdentity();

            identity.AddClaim(new Claim(Global.ClaimTypes.UserId, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            identity.AddClaim(new Claim(Global.ClaimTypes.LoginScret, user.SecurityStamp));
            identity.AddClaim(new Claim(Global.ClaimTypes.AdSoyad, user.Ad));
            identity.AddClaim(new Claim(Global.ClaimTypes.LoginId, user.Logins.LastOrDefault()?.Id.ToString() ?? string.Empty));

            return identity;
        }

        public static ClaimsIdentity? GetUserClaims(string guid, int userId)
        {
            var d = _cache.Get(guid);
            var identity = _cache.GetOrCreate<ClaimsIdentity>(guid, entry =>
            {
                //entry.AbsoluteExpirationRelativeToNow = Global.TokenKeys.SimdidenGecerlilikSure;
                entry.SlidingExpiration = Global.TokenKeys.KayanGecerlilik;
                return CreateUserClaims(userId);
            });
            if (identity == null) return null;
            if (identity.Claims.FirstOrDefault(i => i.Type == Global.ClaimTypes.UserId)?.Value.TryToInt() != userId) return null;
            return identity;
        }

        public static void AttachUserToContextAsync(JwtSettings appSettings, HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == Global.ClaimTypes.UserId).Value);
                var GuidKey = jwtToken.Claims.First(x => x.Type == Global.TokenKeys.GuidKey).Value;
                var claims = jwtToken.Claims.ToList();

                context.Items[Global.TokenKeys.GuidKey] = GuidKey;
                context.Items[Global.TokenKeys.Claims] = claims;
                context.Items[Global.ClaimTypes.UserId] = userId;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
        public static SecurityToken? AttachUserToContextAsync(JwtSettings appSettings, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                _ = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                return validatedToken;
            }
            catch
            {
                return null;
            }
        }
    }
}
