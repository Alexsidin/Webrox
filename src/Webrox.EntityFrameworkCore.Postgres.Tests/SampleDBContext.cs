using Microsoft.EntityFrameworkCore;

namespace Webrox.EntityFrameworkCore.Postgres.Tests
{
    internal class SampleDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public SampleDbContext()
        {
        }

        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(opt =>
            {
                opt.HasKey(e => e.Id);
            });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, RoleId = 1, Email = "sample1@gm.com" },
                new User { Id = 2, RoleId = 3, Email = "sample2@gm.com" },
                new User { Id = 3, RoleId = 2, Email = "sample3@gm.com" },
                new User { Id = 5, RoleId = 2, Email = "sample5@gm.com" },
                new User { Id = 6, RoleId = 1, Email = "sample6@gm.com" },
                new User { Id = 7, RoleId = 2, Email = "sample7@gm.com" },
                new User { Id = 8, RoleId = 3, Email = "sample8@gm.com" },
                new User { Id = 20, RoleId = 1, Email = "sample20@gm.com" },
                new User { Id = 21, RoleId = 3, Email = "sample21@gm.com" },
                new User { Id = 30, RoleId = 1, Email = "sample30@gm.com" }

                );
        }
    }

    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }

        public string Email { get; set; }
    }
}
