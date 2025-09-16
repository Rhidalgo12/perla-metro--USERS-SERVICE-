using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PerlaMetro.data;
using PerlaMetro.src.models.User;
using Bogus;

namespace PerlaMetro.src.data
{
    public class DataSeeder
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            
            var faker = new Faker("es");

            if (!await userManager.Users.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    var name = faker.Name.FirstName();
                    var lastName = faker.Name.LastName();

                    var email = $"{name.ToLower()}.{lastName.ToLower()}@perlametro.cl";

                    if (await userManager.FindByEmailAsync(email) != null)
                        continue;

                    var user = new AppUser
                    {
                        Id = Guid.NewGuid(), 
                        UserName = email,
                        Email = email,
                        Name = $"{name}",
                        LastName = $"{lastName}",
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow
                    };

                    
                    string password = "Passw0rd2!";

                    var createUser = await userManager.CreateAsync(user, password);

                    if (!createUser.Succeeded)
                    {
                        foreach (var error in createUser.Errors)
                        {
                            Console.WriteLine($"Error al crear {email}: {error.Description}");
                        }
                        continue;
                    }

                    var roleResult = await userManager.AddToRoleAsync(user, "User");
                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine($"Usuario {user.Email} creado exitosamente");
                    }
                    else
                    {
                        Console.WriteLine($"Error al asignar rol a {user.Email}");
                    }
                }

                var adminEmail = "admin@perlametro.cl";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var admin = new AppUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = adminEmail,
                        Email = adminEmail,
                        Name = "Administrador",
                        LastName = "PerlaMetro",
                        IsActive = 1,
                        CreatedAt = DateTime.UtcNow
                    };

                    var adminResult = await userManager.CreateAsync(admin, "Admin1234!");
                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        Console.WriteLine("Administrador creado correctamente");
                    }
                    else
                    {
                        foreach (var error in adminResult.Errors)
                        {
                            Console.WriteLine($"Error Admin: {error.Description}");
                        }
                    }
                }
            }
        }
    }
}
