using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ILanni.Common.Identity
{
    public class User : IdentityUser<string>
    {
        //public List<UserRole> Roles { get; set; } = new List<UserRole>();
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
