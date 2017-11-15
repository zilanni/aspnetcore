using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ILanni.Common.User.DbModel
{
    public class User : IdentityUser<string>
    {
        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
