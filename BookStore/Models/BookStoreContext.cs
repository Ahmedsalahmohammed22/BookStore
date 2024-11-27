using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models
{
    public class BookStoreContext : IdentityDbContext
    {
        public BookStoreContext()
        {

        }
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<OrderDetails> OrdersDetails { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrderDetails>()
                .HasKey(od => new { od.book_id, od.order_id });
            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Name = "admin", NormalizedName = "ADMIN" },
                    new IdentityRole { Name = "customer", NormalizedName = "CUSTOMER" }
                );


        }
    }
}
