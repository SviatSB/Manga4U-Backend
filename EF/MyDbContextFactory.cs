using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DATAINFRASTRUCTURE
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\godzi\\Desktop\\Manga4U-Backend\\DB\\app.db");

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
