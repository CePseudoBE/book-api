using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Books)
                .WithMany(b => b.Users)
                .UsingEntity(j => j.ToTable("user_book"));

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            modelBuilder.Entity<Review>()
                .HasOne(b => b.Book)
                .WithMany(a => a.Reviews)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Review>()
                .HasOne(b => b.User)
                .WithMany(a => a.Reviews)
                .HasForeignKey(b => b.UserId);        
        }
    }
}
