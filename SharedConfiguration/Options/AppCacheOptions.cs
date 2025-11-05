using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

using SharedConfiguration.Interfaces;

namespace SharedConfiguration.Options
{
    public class AppCacheOptions : IAppOptions
    {
        public int AbsoluteExpirationRelativeToNow { get; set; }
        public int SlidingExpiration { get; set; }
        public CacheItemPriority Priority { get; set; }
    }
}
