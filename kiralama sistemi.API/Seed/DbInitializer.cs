using kiralamaSistemi.DataAccess.Extensions;
using kiralamaSistemi.DataAccess.Sevices;
using kiralamaSistemi.DataAccess.Concrete;
using kiralamaSistemi.Entities.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kiralamaSistemi.API.Seed
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IApplicationBuilder app)
        {
            try
            {


                using var serviceScope = app.ApplicationServices.CreateScope();
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                var _userManager = serviceScope.ServiceProvider.GetRequiredService<AspNetUserManager<AppUser>>();


                if (context == null) return;

                context.Database.EnsureCreated();

                //Create Super Admin Role
                var superAdminRole = await context.Roles.FirstOrDefaultAsync(i => i.Name == "SuperAdmin");
                if (superAdminRole == null)
                {
                    superAdminRole = new AppRole
                    {
                        Name = "SuperAdmin",
                        CreatedByName = "System",
                        Locked = true,
                    };
                    await context.AddAsync(superAdminRole);
                }

                //Create Admin Role
                var adminRole = await context.Roles.FirstOrDefaultAsync(i => i.Name == "Admin");
                if (adminRole == null)
                {
                    adminRole = new AppRole
                    {
                        Name = "Admin",
                        CreatedByName = "System",
                        Locked = true,
                    };
                    await context.Roles.AddAsync(adminRole);
                    await context.SaveChangesAsync();
                }

                //Create superuser   
                var superuser = await context.Users.FirstOrDefaultAsync(i => i.UserName == "superadmin@test.com");
                if (superuser == null)
                {
                    superuser = new AppUser
                    {
                        Email = "superadmin@test.com",
                        UserName = "superadmin@test.com",
                        EmailConfirmed = true,
                        State = true,
                        Ad = "Abdulilah",
                        Soyad = "Elnağme",
                        AppUserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = superAdminRole.Id
                            }
                        }
                    };
                    var fsd = await _userManager.CreateAsync(superuser, "admin.123");
                    await context.SaveChangesAsync();
                }

                //Create user   
                var user = await context.Users.FirstOrDefaultAsync(i => i.UserName == "admin@test.com");
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = "admin@test.com",
                        UserName = "admin@test.com",
                        EmailConfirmed = true,
                        State = true,
                        Ad = "Abdulilah",
                        Soyad = "Elnağme",
                        AppUserRoles = new List<AppUserRole>
                        {
                            new AppUserRole
                            {
                                RoleId = adminRole.Id
                            }
                        }
                    };
                    var fsd = await _userManager.CreateAsync(user, "admin.123");
                    await context.SaveChangesAsync();
                }


            }

            catch (OzelException) { throw; }
            catch (Exception ex)
            {
                ex.WriteToXml(new DataAccess.Sevices.Error($"{nameof(API)}- {nameof(Seed)} - {nameof(DbInitializer)} - {nameof(InitializeAsync)}"));
                throw new OzelException(ErrorProvider.APIHatasi);
            }
        }
    }
}
