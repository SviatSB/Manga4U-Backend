using System.Reflection.Emit;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>(builder =>
            {
                builder
                    .HasOne(c => c.RepliedComment)
                    .WithMany(c => c.RepliesComments)
                    .HasForeignKey(c => c.RepliedCommentId)
                    .OnDelete(DeleteBehavior.Cascade);

                //builder
                //    .Property(c => c.CreationTime)
                //    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Review>(builder =>
            {
                builder
                    .HasIndex(r => new { r.UserId, r.MangaId })
                    .IsUnique();

                //builder
                //    .Property(r => r.CreationTime)
                //    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            modelBuilder.Entity<Collection>(builder =>
            {
                builder
                    .HasMany(c => c.Mangas)
                    .WithMany(m => m.Collections)
                    .UsingEntity(j =>
                        j.HasIndex(
                            new[] { "CollectionsId", "MangasId" }
                        ).IsUnique());

                //builder
                //    .Property(r => r.CreationTime)
                //    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<User>(builder =>
            {
                //builder
                //    .Property(r => r.RegistrationTime)
                //    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
