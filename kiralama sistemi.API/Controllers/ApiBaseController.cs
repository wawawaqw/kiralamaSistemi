using kiralamaSistemi.DataAccess.Concrete;
using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.Entities.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace kiralamaSistemi.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiBaseController : ControllerBase, IAsyncActionFilter//, IActionFilter
    {
        protected readonly JwtSettings _jwtSettings;
        protected readonly IWebHostEnvironment _env;

        public ApiBaseController(
            IWebHostEnvironment env,
            IOptions<JwtSettings> jwtSettings)
        {
            _env = env;
            _jwtSettings = jwtSettings.Value;
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task OnActionExecutedAsync(ActionExecutedContext context)
        {

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<bool> OnActionExecutingAsync(ActionExecutingContext actionContext)
        {
            try
            {
                bool hasAllowAnonymous = (actionContext.ActionDescriptor.EndpointMetadata
                                            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute))
                                        && actionContext.ActionDescriptor.EndpointMetadata
                                            .All(em => em.GetType() != typeof(RequredAuthorizeAttribute)))
                                        || actionContext.ActionDescriptor.EndpointMetadata
                                            .All(em => em.GetType() != typeof(AuthorizeAttribute));

                if (!hasAllowAnonymous) // when action is not allow anonymous 
                {
                    var LoginScret = User.GetLoginScret();

                    var id = User.GetUserId();
                    var context = new AppDbContext();
                    var user = id != 0 ? await context.Users.FirstOrDefaultAsync(i => i.Id == id) : null;

                    if (user == null)
                    {
                        actionContext.Result = new JsonResult(ErrorProvider.Auth.NotAuthenticated) { StatusCode = StatusCodes.Status401Unauthorized };
                        return false;
                    }

                    //var tt = TokenHelper.GetUserClaims(User.GetGuidKey(),User.GetUserId());
                    if (user.SecurityStamp != LoginScret) // if user is not deleted or SecurityStamp has changed 
                    {
                        if (!user?.State ?? false)
                        {
                            actionContext.Result = new JsonResult(ErrorProvider.Account.AccountIsNotActive) { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                        else
                        {
                            actionContext.Result = new JsonResult(ErrorProvider.Auth.FarkliLogin) { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error("API-Controllers-OnActionExecutionAsync"));
                return false;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task OnActionExecutionAsync([FromBody] ActionExecutingContext context,
                                         [FromRoute] ActionExecutionDelegate next)
        {
            try
            {
                if (await OnActionExecutingAsync(context))
                {
                    var resultContext = await next();
                    await OnActionExecutedAsync(resultContext);
                }
            }
            catch (OzelException) { return; }
            catch (Exception ex)
            {
                ex.WriteToXml(new Error("API-Controllers-OnActionExecutionAsync"));
                context.Result = new JsonResult(ErrorProvider.APIHatasi) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
