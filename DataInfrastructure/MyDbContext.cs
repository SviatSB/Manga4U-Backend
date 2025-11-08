using Domain.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure
{
    public class MyDbContext : IdentityDbContext<User, IdentityRole<long>, long>
    {
        //дефолты, юники, каскадное удаление, лейзи лоадинг = фолс

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<Manga> Mangas { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
