using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PerlaMetro.src.models.User
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? Name { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public override Guid Id { get; set; } = Guid.NewGuid();
    }
}