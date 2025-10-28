using DATAINFRASTRUCTURE;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WEBAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseDeveloperFeatures(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        public static WebApplication UseAppRequestPipeline(this WebApplication app, string corsPolicy)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(corsPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
