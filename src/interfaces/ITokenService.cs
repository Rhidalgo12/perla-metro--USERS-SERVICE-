using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerlaMetro.src.models.User;

namespace PerlaMetro.src.interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}