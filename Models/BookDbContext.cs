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
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PasswordReset>()
                .HasIndex(pr => pr.Token)
                .IsUnique();
            
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
                .WithMany(r => r.Reviews)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<PasswordReset>()
                .HasOne(pr => pr.User)
                .WithMany(a => a.PasswordResets)
                .HasForeignKey(pr => pr.UserId);        
        }
    }
}
