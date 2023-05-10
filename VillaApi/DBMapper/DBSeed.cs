using CoreModule.DbContextConfig;
using CoreModule.Src;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace VillaApi.DBMapper
{
    public class DBSeed
    {

        public  static async Task  Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(User.RoleAdmin))
                    await roleManager.CreateAsync(new IdentityRole(User.RoleAdmin));
                if (!await roleManager.RoleExistsAsync(User.RoleUser))
                    await roleManager.CreateAsync(new IdentityRole(User.RoleUser));
                await context.SaveChangesAsync();
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@admin.com";
                string adminUserName = "admin";

                var adminUser = await userManager.FindByNameAsync(adminUserName);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        FullName = "Admin User",
                        UserName = "admin",
                        Email = adminUserEmail
                    };
                   var user = await userManager.CreateAsync(newAdminUser, "admin");
                   
                    await userManager.AddToRoleAsync(newAdminUser, User.RoleAdmin);
            
                }



            }

        }

    }
}
