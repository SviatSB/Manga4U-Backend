using Services.Extensions;

namespace WebApi.Extensions.Temp
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
            app.UseUserActivity();
            app.MapControllers();
            return app;
        }
    }
}
