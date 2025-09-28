using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerlaMetro.src.models.User;

namespace PerlaMetro.data
{
    /// <summary>
    /// Data context for the application, extending IdentityDbContext to include user and role management.
    /// </summary>
    public class DataContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options for the database context.</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        /// <summary> Configures the model by seeding initial data for roles.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// <summary> Calls the base method to ensure Identity configurations are applied.
            /// </summary>
            base.OnModelCreating(modelBuilder);
            List<IdentityRole<Guid>> roles = new List<IdentityRole<Guid>>
            {
                /// <summary>
                /// Role for administrators with full access.
                /// </summary>
                /// <value></value>
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                /// <summary>
                /// Role for regular users with limited access.
                /// </summary>
                new IdentityRole<Guid>
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(roles);
        }
    }
}