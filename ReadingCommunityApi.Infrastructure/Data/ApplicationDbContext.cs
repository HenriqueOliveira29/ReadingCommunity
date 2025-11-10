using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.Book)
                  .WithMany(b => b.Reviews)
                  .HasForeignKey(r => r.BookId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.User)
                  .WithMany(u => u.Reviews)
                  .HasForeignKey(r => r.UserId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Author>(entity =>
       {
           entity.HasKey(a => a.Id);

           entity.HasMany(a => a.Books)
                 .WithOne(b => b.Author)
                 .HasForeignKey(b => b.AuthorId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);
       });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
        });
        
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
    }
}