using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DATAINFRASTRUCTURE
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            optionsBuilder.UseSqlite("Data Source=D:\\MyDir\\sqlite\\manga4u.db");

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
