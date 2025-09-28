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
    /// <summary>
    /// Class responsible for seeding initial data into the database.
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Initializes the database with default users and roles if they do not exist.
        /// </summary>
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            /// <summary>
            /// Creates a scope to obtain scoped services like UserManager and DataContext.
            /// </summary>
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();

            /// <summary>
            /// Creates a Faker instance for generating test data.
            /// </summary>
            var faker = new Faker("es");
            /// <summary>
            /// Checks if there are any users in the database.
            /// </summary>
            /// <param name="userManager.Users.AnyAsync()"></param>
            if (!await userManager.Users.AnyAsync())
            {
                for (int i = 0; i < 10; i++)
                {
                    /// <summary>
                    /// Generates a random user name.
                    /// </summary>
                    /// <returns>A random user name.</returns>
                    var name = faker.Name.FirstName();
                    var lastName = faker.Name.LastName();

                    var email = $"{name.ToLower()}.{lastName.ToLower()}@perlametro.cl";

                    if (await userManager.FindByEmailAsync(email) != null)
                        continue;
                    /// <summary>
                    /// Creates a new AppUser instance with generated data.
                    /// </summary>
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

                    /// <summary>
                    /// The password for the new user.
                    /// </summary>
                    string password = "Passw0rd2!";
                    /// <summary>
                    /// Creates the user with the specified password.
                    /// </summary>
                    var createUser = await userManager.CreateAsync(user, password);
                    /// <summary>
                    /// Checks if the user creation was successful.
                    /// </summary>
                    if (!createUser.Succeeded)
                    {
                        foreach (var error in createUser.Errors)
                        {
                            Console.WriteLine($"Error al crear {email}: {error.Description}");
                        }
                        continue;
                    }
                    /// <summary>
                    /// Assigns the "User" role to the newly created user.
                    /// </summary>
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
                /// <summary> Creates an admin user if it does not exist.
                /// </summary>
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
                    /// <summary> Creates the admin user with a predefined password.
                    /// </summary>
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
