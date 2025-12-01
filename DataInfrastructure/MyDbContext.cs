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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Comment>()
                .HasOne(c => c.RepliedComment)
                .WithMany(c => c.RepliesComments)
                .HasForeignKey(c => c.RepliedCommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasIndex(r => new { r.UserId, r.MangaId })
                .IsUnique();

            //builder.Entity<Collection>(builder =>
            //{
            //    builder.HasIndex(c => new { c.UserId, c.MangaId })
            //        .IsUnique();

            //    builder.Property(c => c.);
            //});
        }
    }
}
