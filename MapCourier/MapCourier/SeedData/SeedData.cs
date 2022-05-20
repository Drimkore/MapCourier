using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MapCourier.Data;

namespace MapCourier.SeedData

{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MapContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MapContext>>()))
            {
                if (context.Roles.Any() && context.Users.Any())
                {
                    return;
                }
                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    },
                    new IdentityRole
                    {
                        Name = "Operator",
                        NormalizedName = "OPERATOR"
                    },
                    new IdentityRole
                    {
                        Name = "Courier",
                        NormalizedName = "COURIER"
                    }
                );  
                /*PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
                
                context.Users.AddRange(
                    new IdentityUser(){
                        UserName = "admin",
                        Email = "admin@mail.ru",
                        EmailConfirmed = true,
                        PasswordHash = userManager.CreateAsync()
                    }
                )       */      
                context.SaveChanges();
            }
        }
    
       /* private static void Psswd(UserManager<IdentityUser> userManager){
            IdentityUser identityUser = new IdentityUser(){
                UserName = "admin",
                Email = "admin@mail.ru",
                EmailConfirmed = true
            };
            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            passwordHasher.HashPassword(identityUser, "Admin1!");
            userManager.CreateAsync(identityUser, "Admin1!").Result;            
        }*/
    }
}
