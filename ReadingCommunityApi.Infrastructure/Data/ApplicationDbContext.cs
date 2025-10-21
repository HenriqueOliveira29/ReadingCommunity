using Microsoft.EntityFrameworkCore;
using ReadingCommunityApi.Core.Models;

namespace ReadingCommunityApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                  .OnDelete(DeleteBehavior.Restrict);
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
        
        base.OnModelCreating(modelBuilder);
    }
}