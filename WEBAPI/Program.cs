using EF;
using Microsoft.Extensions.Configuration;

namespace WEBAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //==MyServices==

            string conString;
            if (builder.Configuration.GetValue<bool>("Config:UseInMemoryDB"))
            {
                conString = builder.Configuration.GetConnectionString("InMemoryConnection");
            }
            else
            {
                conString = builder.Configuration.GetConnectionString("InMemoryConnection");
            }
            builder.Services.DataBaseDI(conString);

            //==MyServices==

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
