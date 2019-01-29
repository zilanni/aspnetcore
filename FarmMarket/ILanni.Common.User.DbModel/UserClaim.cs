using System;
using System.Collections.Generic;
using System.Text;

namespace ILanni.Common.User.DbModel
{
     public class UserClaim
    {
        public int Id { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public string UserId { get; set; }
    }
}
