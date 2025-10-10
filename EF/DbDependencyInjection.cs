using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF
{
    public static class DbDependencyInjection
    {
        public static IServiceCollection Add(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDbContext>(options => options.UseSqlite(connectionString));

            return services;
        }
    }
}
