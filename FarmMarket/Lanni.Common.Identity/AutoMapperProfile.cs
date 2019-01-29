using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using AutoMapper;

namespace ILanni.Common.Identity
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ILanni.Common.User.DbModel.User, User>();
            CreateMap<User, ILanni.Common.User.DbModel.User>();
            CreateMap<ILanni.Common.User.DbModel.UserRole, Role>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.RoleId))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.RoleId))
                .ForMember(d => d.NormalizedName, opts => opts.MapFrom(s => s.RoleId))
                .ForMember(d => d.ConcurrencyStamp, opts => opts.Ignore());
        }
    }
}
