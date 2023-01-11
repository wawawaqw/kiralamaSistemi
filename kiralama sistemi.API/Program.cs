using kiralamaSistemi.API.Seed;
using kiralamaSistemi.Entities.Abstract;
using kiralamaSistemi.DataAccess.Concrete;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Globalization;
using kiralamaSistemi.DataAccess.Sevices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using kiralamaSistemi.DataAccess.Abstract;
using kiralamaSistemi.DataAccess.Concrete.Repositories;
using kiralama_sistemi.DataAccess.Concrete.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
ConfigurationManager configuration = builder.Configuration;
services.AddDbContext<AppDbContext>();

services.AddIdentity<AppUser, AppRole>(
              opt =>
              {
                  //previous code removed for clarity reasons
                  opt.Lockout.AllowedForNewUsers = true;
                  opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                  opt.Lockout.MaxFailedAccessAttempts = 10;
              }).AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = false;
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
}).AddUserManager<AspNetUserManager<AppUser>>();

//App Settings
services.AddScoped<JwtSettings>();
var appSettingsSection = builder.Configuration.GetSection("JWT");
services.Configure<JwtSettings>(appSettingsSection);

// token time out life span 
services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromDays(1);
});


// Add services to the container.
//Session and Cache
services.AddDistributedMemoryCache();
services.AddMemoryCache();
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddTransient<IEmailSender, EmailSender>();
services.AddResponseCompression();
services.AddScoped<Global>();

//Data Access Scopes -- Özel --
services.AddScoped<IAccountRepository, AccountRepository>();
services.AddScoped<IServiceRepository, ServiceRepository>();

////Data Access Scopes -- Genel --
services.AddSingleton<IRepository<Log>, GenericRepository<Log>>();
services.AddSingleton<IRepository<Login>, GenericRepository<Login>>();
services.AddSingleton<IRepository<LogModuleMap>, GenericRepository<LogModuleMap>>();
services.AddSingleton<IRepository<Musteri>, GenericRepository<Musteri>>();
services.AddSingleton<IRepository<AppUserRole>, GenericRepository<AppUserRole>>();
services.AddSingleton<IRepository<AppUser>, GenericRepository<AppUser>>();
services.AddSingleton<IRepository<AppRole>, GenericRepository<AppRole>>();
services.AddSingleton<IRepository<Tarife>, GenericRepository<Tarife>>();
services.AddSingleton<IRepository<Kiralama>, GenericRepository<Kiralama>>();
services.AddSingleton<IRepository<Araba>, GenericRepository<Araba>>();

// Adding JWT Authentication  
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        },
        OnForbidden = ctx =>
        {
            var events = new JwtBearerEvents();
            return events.Forbidden(ctx);
        },
    };
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Global.JWTKeys.Secret])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidAudience = builder.Configuration[Global.JWTKeys.ValidAudience],
        ValidIssuer = builder.Configuration[Global.JWTKeys.ValidIssuer],
    };
});

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "kiralama Sistemi",
        Description = "Pigment Soft Yazılım Ve Bilişim Şirketin Sitesi",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Bize Ulaşın",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Lisans Örneği",
            Url = new Uri("https://example.com/license")
        }
    });

    c.DescribeAllParametersInCamelCase();
    c.UseOneOfForPolymorphism();
    c.UseAllOfForInheritance();
    c.EnableAnnotations();

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Using the Authorization header with the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },

    };

    c.AddSecurityDefinition("Bearer", securitySchema);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securitySchema, new[] { "Bearer" } },
        });

});


services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000",
                    "https://site.param.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithMethods("PUT", "DELETE", "GET", "POST");
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowCredentials();
        });
});


services.AddControllers().AddNewtonsoftJson(options =>
{
    options.AllowInputFormatterExceptionMessages = true;
    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.FloatFormatHandling = Newtonsoft.Json.FloatFormatHandling.DefaultValue;
});

services.AddHttpContextAccessor().AddAuthentication();

services.AddEndpointsApiExplorer();



var app = builder.Build();

Global.Folders.CreateFolders();


var cultureInfo = new CultureInfo("tr-TR");
cultureInfo.NumberFormat.CurrencySymbol = "₺";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

TokenHelper.TokenHelperConfigure(app.Services.GetService<IMemoryCache>());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pigment Soft Site API v1");
        c.RoutePrefix = String.Empty;
    });

    await DbInitializer.InitializeAsync(app);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
    c.RoutePrefix = "swagger";

    // Display
    c.DefaultModelExpandDepth(5);
    c.DefaultModelRendering(ModelRendering.Example);
    c.DefaultModelsExpandDepth(5);
    c.DisplayOperationId();
    c.DisplayRequestDuration();
    c.DocExpansion(DocExpansion.None);
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();

    // Network
    c.EnableValidator();
    c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);

    c.EnablePersistAuthorization();
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();



