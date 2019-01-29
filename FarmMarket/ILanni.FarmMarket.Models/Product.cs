using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILanni.FarmMarket.Models
{
    public class Product
    {
        public long Id { get; set; }

        public long Storeid { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public int Status { get; set; }

        public string Summary { get; set; }

        public string Desc { get; set; }

        public List<string> Area { get; set; }

        public List<string> Areacode { get; set; }

        public List<string> Category { get; set; }

        public List<string> Categorycode { get; set; }

        public List<Tag> Tag { get; set; }

        public float[] Position { get; set; }

        public string Sptype { get; set; }

        public List<string> Keywords { get; set; }

    }

    public class Tag
    {
        public string Name { get; set; }

        public string Val { get; set; }
    }
}
