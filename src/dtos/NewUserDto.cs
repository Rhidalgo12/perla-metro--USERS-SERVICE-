using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerlaMetro.src.dtos
{
    public class NewUserDto
    {
       
        public string UserName { get; set; } = null!;

       
        public string Email { get; set; } = null!;

       
        public string Token { get; set; } = null!;
    }
}