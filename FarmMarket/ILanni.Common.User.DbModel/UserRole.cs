using System;
using System.Collections.Generic;
using System.Text;

namespace ILanni.Common.User.DbModel
{
    public class UserRole
    {
        public int Id { get; set; }

        public User User { get; set; }

        public string RoleId { get; set; }
    }
}
