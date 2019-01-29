using System;
using System.Collections.Generic;
using System.Text;

namespace ILanni.Common.User.DbModel
{
    public class UserTag
    {
        public long Id { get; set; }

        public string Tag { get; set; }

        public int Version { get; set; }
    }
}
