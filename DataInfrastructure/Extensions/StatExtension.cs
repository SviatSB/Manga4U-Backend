using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Interfaces;
using DataInfrastructure.Other;

using Microsoft.Extensions.DependencyInjection;

namespace DataInfrastructure.Extensions
{
    public static class StatExtension
    {
        public static IServiceCollection AddStatQuery(this IServiceCollection services)
        {
            return services.AddScoped<IStatQueryService, StatQueryService>();
        }
    }
}
