using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Domain.Identity;
using OnlineLibrary.Domain.Models.BaseModels;
using OnlineLibrary.Domain.Models.RelationalModels;

namespace OnlineLibrary.Repository
{
    public class ApplicationDbContext : IdentityDbContext<Member>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<BorrowingCart> BorrowingCarts { get; set; }
        public DbSet<BorrowingHistory> BorrowingHistories { get; set; }
        public DbSet<BookInBorrowingCart> BookInBorrowingCarts { get; set; }
        public DbSet<BookInBorrowingHistory> BookInBorrowingHistories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
