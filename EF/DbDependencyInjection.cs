using DATAINFRASTRUCTURE.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATAINFRASTRUCTURE
{
    public static class DbDependencyInjection
    {
        public static IServiceCollection AddDataBaseDI(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDbContext>(options => options.UseSqlite(connectionString));

            services.AddScoped<UserRepository>();
            services.AddScoped<MangaRepository>();
            services.AddScoped<CollectionRepository>();

            return services;
        }
    }
}
