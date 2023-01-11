using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.Entities.Tables;
using LokantaSisteme_API.Models.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using kiralama_sistemi.DataAccess.Sevices;

namespace kiralamaSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ApiBaseController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IServiceRepository _service;

        public AccountController(UserManager<AppUser> userManager,
                                 IAccountRepository accountRepository,
                                 IServiceRepository service,
                                 ILogger<AccountController> logger,
                                 IOptions<JwtSettings> jwtSettings,
                                 IWebHostEnvironment env) : base(env, jwtSettings)
        {
            _userManager = userManager;
            _accountRepository = accountRepository;
            _service = service;
            _logger = logger;
        }



        [HttpGet]
        [Route("Logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User.Claims.Any())
                {
                    await _accountRepository.LogoutAsync(User);
                }
                return Ok();
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"API-Controllers-Account-{nameof(Logout)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            try
            {
                var entity = new AppUser()
                {
                    Ad = model.Ad,
                    Email = model.Email,
                    Soyad=model.Soyad,
                    EmailConfirmed = true,
                    UserName = model.Email,
                    State = model.State,
                    Musteri = new Musteri()
                    {
                        Telefon = model.Telefon,
                        Cinsiyet=model.Cinsiyet,
                        Tc=model.Tc,
                    }
                };
                await _accountRepository.SignUpAsync(entity, model.Password);

                return Ok(entity);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"{nameof(LokantaSisteme_API)} - {nameof(Controllers)} - {nameof(AccountController)} - {nameof(SignUp)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(LoginModel model)
        {
            try
            {
                bool devMode = false;
                if (_env.IsDevelopment() && (string.IsNullOrWhiteSpace(model.Email) || model.Email == "string"))
                {
                    model = new()
                    {
                        Persistent = true,
                        Email = "admin@test.com",
                        Password = "admin.123",
                    };
                    devMode = true;
                }

                //Check is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ErrorProvider.NotValid);
                }

                // Check is Banned
                string ip = HttpContext.Connection.RemoteIpAddress.ToString();

                //Check Or Store Current Ip
                var checkIp = _service.CheckAndStoreLoginIp(ip);
                if (!checkIp.Status)
                {
                    return BadRequest(checkIp.Errors);
                }
                
                // Find User
                var user = await _userManager.Users.Include(i => i.AppUserRoles)
                                                    .ThenInclude(i => i.Role)
                                                   .FirstOrDefaultAsync(i => i.Email == model.Email)
                                ?? throw new OzelException(ErrorProvider.Auth.InvalidLogin);

                var login = new Login()
                {
                    Browser = "api",
                    UserId = user.Id,
                    UserName = user.UserName,
                    OsType = "api",
                    Date = DateTime.Now,
                    Ip = ip
                };
                await _accountRepository.LoginByEmailAsync(model.Email, model.Password, login);

                //Set Identity Claims
                var claims = new List<Claim>
                    {
                        new Claim(Global.ClaimTypes.UserId, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                    IsPersistent = true,
                };

                var guid = Guid.NewGuid().ToString();

                var token = TokenHelper.GenerateJwtToken(_jwtSettings, user, guid, login?.Id);

                var response = new AuthenticateResponse(user, token);
                return devMode ? Ok(response.Token) : Ok(response);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"API-Controllers-Account-{nameof(Login)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }



        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                await _accountRepository.ResetPasswordEmailAsync(model.Email);

                return Ok(ErrorProvider.Auth.SucceedForgotPass);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"API-Controllers-Account-{nameof(ForgotPassword)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(int userId, string token)
        {
            try
            {
                //token = HttpUtility.UrlDecode(token);
                if (userId == 0)
                {
                    throw new OzelException(ErrorProvider.NotValidParams);
                }
                var user = await _userManager.Users.FirstOrDefaultAsync(i => i.Id == userId);
                if (user == null)
                {
                    throw new OzelException(ErrorProvider.Auth.NotFoundUser);
                }
                var result = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);
                if (!result)
                {
                    throw new OzelException(ErrorProvider.Auth.InvalidToken);
                }
                var model = new ResetPasswordModel()
                {
                    Email = user.UserName,
                    Token = token
                };
                return Ok(model);
            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"API-Controllers-Account-{nameof(ResetPassword)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (model.Password == null)
                {
                    throw new OzelException(ErrorProvider.NotValid);
                }
                else if (model.Password != model.RePassword)
                {
                    throw new OzelException(ErrorProvider.Auth.NotMatchPasses);
                }

                await _accountRepository.ResetPasswordAsync(model.Email, model.Token, model.Password);
                return Ok(ErrorProvider.Auth.SucceedPassChange);

            }
            catch (OzelException ex)
            {
                return BadRequest(ex.GetListError());
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error($"API-Controllers-Account-{nameof(ResetPassword)}"));
                return BadRequest(ErrorProvider.APIHatasi);
            }
        }








    }
}
