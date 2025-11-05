using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedConfiguration.Interfaces;

namespace SharedConfiguration.Options
{
    public class AppProxyOptions : IAppOptions
    {
        public string UserAgent { get; set; } = null!;
    }
}
