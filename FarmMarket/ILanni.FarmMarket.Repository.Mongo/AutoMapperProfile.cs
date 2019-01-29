using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using AutoMapper;

namespace ILanni.FarmMarket.Repository.Mongo
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ILanni.FarmMarket.Models.Product,Product>();
        }
    }
}
