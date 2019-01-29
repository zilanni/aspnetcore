using System;
using System.Collections.Generic;
using System.Linq;

namespace ILanni.Common
{
    public class PageSupport<T>
    {

        public long LongTotalCount { get; set; }

        public int TotalCount
        {
            get
            {
                return unchecked((int)LongTotalCount);
            }
            set
            {
                LongTotalCount = value;
            }
        }

        public IEnumerable<T> Raws { get; set; }

        public PageSupport()
        { }

        public PageSupport(IEnumerable<T> raws,long totalCount)
        {
            this.Raws = raws;
            this.LongTotalCount = totalCount;
        }

        public PageSupport(IEnumerable<T> raws, int totalCount)
        {
            this.Raws = raws;
            this.LongTotalCount = totalCount;
        }

        public static PageSupport<T> Empty()
        {
            return new PageSupport<T>(Enumerable.Empty<T>(), 0);
        }

    }
}
