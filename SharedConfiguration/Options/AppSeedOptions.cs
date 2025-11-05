using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedConfiguration.Interfaces;

namespace SharedConfiguration.Options
{
    public class AppSeedOptions : IAppOptions
    {
        public string OwnerLogin { get; set; } = null!;
        public string OwnerPassword { get; set; } = null!;
    }
}
